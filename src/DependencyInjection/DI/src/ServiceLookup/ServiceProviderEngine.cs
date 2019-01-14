// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 服务提供程序引擎
    /// </summary>
    internal abstract class ServiceProviderEngine : IServiceProviderEngine, IServiceScopeFactory
    {
        /// <summary>
        /// 服务提供程序引擎回调
        /// </summary>
        private readonly IServiceProviderEngineCallback _callback;
        /// <summary>
        /// 创建服务访问者
        /// </summary>
        private readonly Func<Type, Func<ServiceProviderEngineScope, object>> _createServiceAccessor;
        /// <summary>
        /// 是否回收
        /// </summary>
        private bool _disposed;
        /// <summary>
        /// 服务提供程序引擎-构造函数
        /// </summary>
        /// <param name="serviceDescriptors">服务描述集合</param>
        /// <param name="callback">服务提供程序引擎回调</param>

        protected ServiceProviderEngine(IEnumerable<ServiceDescriptor> serviceDescriptors, IServiceProviderEngineCallback callback)
        {
            _createServiceAccessor = CreateServiceAccessor;
            _callback = callback;
            Root = new ServiceProviderEngineScope(this);
            RuntimeResolver = new CallSiteRuntimeResolver();
            CallSiteFactory = new CallSiteFactory(serviceDescriptors);
            CallSiteFactory.Add(typeof(IServiceProvider), new ServiceProviderCallSite());
            CallSiteFactory.Add(typeof(IServiceScopeFactory), new ServiceScopeFactoryCallSite());
            RealizedServices = new ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object>>();
        }

        internal ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object>> RealizedServices { get; }
        /// <summary>
        /// 调用设置工厂
        /// </summary>
        internal CallSiteFactory CallSiteFactory { get; }
        /// <summary>
        /// 调用设置运行时解析器
        /// </summary>
        protected CallSiteRuntimeResolver RuntimeResolver { get; }
        /// <summary>
        /// 服务根节点
        /// </summary>
        public ServiceProviderEngineScope Root { get; }
        /// <summary>
        /// 服务根节点
        /// </summary>
        public IServiceScope RootScope => Root;
        /// <summary>
        /// 验证服务
        /// </summary>
        /// <param name="descriptor">服务描述</param>
        public void ValidateService(ServiceDescriptor descriptor)
        {
            if (descriptor.ServiceType.IsGenericType && !descriptor.ServiceType.IsConstructedGenericType)
            {
                return;
            }

            try
            {
                var callSite = CallSiteFactory.GetCallSite(descriptor, new CallSiteChain());
                if (callSite != null)
                {
                    _callback?.OnCreate(callSite);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error while validating the service descriptor '{descriptor}': {e.Message}", e);
            }
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType) => GetService(serviceType, Root);
        /// <summary>
        /// 实现服务
        /// </summary>
        /// <param name="callSite">服务调用设置</param>
        /// <returns></returns>
        protected abstract Func<ServiceProviderEngineScope, object> RealizeService(ServiceCallSite callSite);
        /// <summary>
        ///回收资源
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            Root.Dispose();
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="serviceProviderEngineScope">服务提供程序引擎范围</param>
        /// <returns></returns>
        internal object GetService(Type serviceType, ServiceProviderEngineScope serviceProviderEngineScope)
        {
            if (_disposed)
            {
                ThrowHelper.ThrowObjectDisposedException();
            }

            var realizedService = RealizedServices.GetOrAdd(serviceType, _createServiceAccessor);
            _callback?.OnResolve(serviceType, serviceProviderEngineScope);
            DependencyInjectionEventSource.Log.ServiceResolved(serviceType);
            return realizedService.Invoke(serviceProviderEngineScope);
        }
        /// <summary>
        /// 创建服务范围
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            if (_disposed)
            {
                ThrowHelper.ThrowObjectDisposedException();
            }

            return new ServiceProviderEngineScope(this);
        }
        /// <summary>
        /// 创建服务访问者
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        private Func<ServiceProviderEngineScope, object> CreateServiceAccessor(Type serviceType)
        {
            var callSite = CallSiteFactory.GetCallSite(serviceType, new CallSiteChain());
            if (callSite != null)
            {
                DependencyInjectionEventSource.Log.CallSiteBuilt(serviceType, callSite);
                _callback?.OnCreate(callSite);
                return RealizeService(callSite);
            }

            return _ => null;
        }
    }
}
