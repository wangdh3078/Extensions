using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 表达式树服务提供程序引擎
    /// </summary>
    internal class ExpressionsServiceProviderEngine : ServiceProviderEngine
    {
        /// <summary>
        /// 表达式解析器生成器
        /// </summary>
        private readonly ExpressionResolverBuilder _expressionResolverBuilder;
        /// <summary>
        /// 表达式树服务提供程序引擎-构造函数
        /// </summary>
        /// <param name="serviceDescriptors">服务描述集合</param>
        /// <param name="callback">服务提供程序引擎回调</param>
        public ExpressionsServiceProviderEngine(IEnumerable<ServiceDescriptor> serviceDescriptors, IServiceProviderEngineCallback callback) : base(serviceDescriptors, callback)
        {
            _expressionResolverBuilder = new ExpressionResolverBuilder(RuntimeResolver, this, Root);
        }
        /// <summary>
        /// 实现服务
        /// </summary>
        /// <param name="callSite">服务调用设置</param>
        /// <returns></returns>
        protected override Func<ServiceProviderEngineScope, object> RealizeService(ServiceCallSite callSite)
        {
            var realizedService = _expressionResolverBuilder.Build(callSite);
            RealizedServices[callSite.ServiceType] = realizedService;
            return realizedService;
        }
    }
}
