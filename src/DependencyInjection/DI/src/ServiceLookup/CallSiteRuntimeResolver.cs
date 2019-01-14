using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 调用设置运行时解析器
    /// </summary>
    internal sealed class CallSiteRuntimeResolver : CallSiteVisitor<RuntimeResolverContext, object>
    {
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="callSite">服务调用设置</param>
        /// <param name="scope">服务提供程序引擎范围</param>
        /// <returns></returns>
        public object Resolve(ServiceCallSite callSite, ServiceProviderEngineScope scope)
        {
            return VisitCallSite(callSite, new RuntimeResolverContext
            {
                Scope = scope
            });
        }
        /// <summary>
        /// 访问回收缓存
        /// </summary>
        /// <param name="transientCallSite">服务调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitDisposeCache(ServiceCallSite transientCallSite, RuntimeResolverContext context)
        {
            return context.Scope.CaptureDisposable(VisitCallSiteMain(transientCallSite, context));
        }
        /// <summary>
        /// 访问构造函数
        /// </summary>
        /// <param name="constructorCallSite">构造函数调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitConstructor(ConstructorCallSite constructorCallSite, RuntimeResolverContext context)
        {
            object[] parameterValues;
            if (constructorCallSite.ParameterCallSites.Length == 0)
            {
                parameterValues = Array.Empty<object>();
            }
            else
            {
                parameterValues = new object[constructorCallSite.ParameterCallSites.Length];
                for (var index = 0; index < parameterValues.Length; index++)
                {
                    parameterValues[index] = VisitCallSite(constructorCallSite.ParameterCallSites[index], context);
                }
            }

            try
            {
                return constructorCallSite.ConstructorInfo.Invoke(parameterValues);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                // The above line will always throw, but the compiler requires we throw explicitly.
                throw;
            }
        }
        /// <summary>
        /// 访问根节点缓存
        /// </summary>
        /// <param name="singletonCallSite">单利服务调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitRootCache(ServiceCallSite singletonCallSite, RuntimeResolverContext context)
        {
            return VisitCache(singletonCallSite, context, context.Scope.Engine.Root, RuntimeResolverLock.Root);
        }
        /// <summary>
        /// 获取范围内缓存
        /// </summary>
        /// <param name="singletonCallSite">单利服务调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitScopeCache(ServiceCallSite singletonCallSite, RuntimeResolverContext context)
        {
            // Check if we are in the situation where scoped service was promoted to singleton
            // and we need to lock the root
            var requiredScope = context.Scope == context.Scope.Engine.Root ?
                RuntimeResolverLock.Root :
                RuntimeResolverLock.Scope;

            return VisitCache(singletonCallSite, context, context.Scope, requiredScope);
        }
        /// <summary>
        /// 访问缓存
        /// </summary>
        /// <param name="scopedCallSite">服务调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <param name="serviceProviderEngine">服务提供程序引擎范围</param>
        /// <param name="lockType">运行时解析器锁</param>
        /// <returns></returns>
        private object VisitCache(ServiceCallSite scopedCallSite, RuntimeResolverContext context, ServiceProviderEngineScope serviceProviderEngine, RuntimeResolverLock lockType)
        {
            bool lockTaken = false;
            var resolvedServices = serviceProviderEngine.ResolvedServices;

            // Taking locks only once allows us to fork resolution process
            // on another thread without causing the deadlock because we
            // always know that we are going to wait the other thread to finish before
            // releasing the lock
            if ((context.AcquiredLocks & lockType) == 0)
            {
                Monitor.Enter(resolvedServices, ref lockTaken);
            }

            try
            {
                if (!resolvedServices.TryGetValue(scopedCallSite.Cache.Key, out var resolved))
                {
                    resolved = VisitCallSiteMain(scopedCallSite, new RuntimeResolverContext
                    {
                        Scope = serviceProviderEngine,
                        AcquiredLocks = context.AcquiredLocks | lockType
                    });

                    serviceProviderEngine.CaptureDisposable(resolved);
                    resolvedServices.Add(scopedCallSite.Cache.Key, resolved);
                }

                return resolved;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(resolvedServices);
                }
            }
        }
        /// <summary>
        /// 访问具体实例
        /// </summary>
        /// <param name="constantCallSite">具体实例调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitConstant(ConstantCallSite constantCallSite, RuntimeResolverContext context)
        {
            return constantCallSite.DefaultValue;
        }
        /// <summary>
        /// 访问服务提供程序
        /// </summary>
        /// <param name="serviceProviderCallSite">服务提供程序调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, RuntimeResolverContext context)
        {
            return context.Scope;
        }
        /// <summary>
        /// 访问服务范围工厂
        /// </summary>
        /// <param name="serviceScopeFactoryCallSite">服务范围工厂调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitServiceScopeFactory(ServiceScopeFactoryCallSite serviceScopeFactoryCallSite, RuntimeResolverContext context)
        {
            return context.Scope.Engine;
        }
        /// <summary>
        /// 访问可枚举
        /// </summary>
        /// <param name="enumerableCallSite">可枚举调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitIEnumerable(IEnumerableCallSite enumerableCallSite, RuntimeResolverContext context)
        {
            var array = Array.CreateInstance(
                enumerableCallSite.ItemType,
                enumerableCallSite.ServiceCallSites.Length);

            for (var index = 0; index < enumerableCallSite.ServiceCallSites.Length; index++)
            {
                var value = VisitCallSite(enumerableCallSite.ServiceCallSites[index], context);
                array.SetValue(value, index);
            }
            return array;
        }
        /// <summary>
        /// 访问工厂
        /// </summary>
        /// <param name="factoryCallSite">工厂调用设置</param>
        /// <param name="context">运行时解析上下文</param>
        /// <returns></returns>
        protected override object VisitFactory(FactoryCallSite factoryCallSite, RuntimeResolverContext context)
        {
            return factoryCallSite.Factory(context.Scope);
        }
    }
    /// <summary>
    /// 运行时解析上下文
    /// </summary>
    internal struct RuntimeResolverContext
    {
        /// <summary>
        /// 服务提供程序引擎范围
        /// </summary>
        public ServiceProviderEngineScope Scope { get; set; }
        /// <summary>
        /// 运行时解析器锁
        /// </summary>
        public RuntimeResolverLock AcquiredLocks { get; set; }
    }
    /// <summary>
    /// 运行时解析器锁
    /// </summary>
    [Flags]
    internal enum RuntimeResolverLock
    {
        /// <summary>
        /// 指定范围
        /// </summary>
        Scope = 1,
        /// <summary>
        /// 根节点
        /// </summary>
        Root = 2
    }
}
