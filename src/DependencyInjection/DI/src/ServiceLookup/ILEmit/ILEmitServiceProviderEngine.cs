// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// IL服务提供程序引擎
    /// </summary>
    internal class ILEmitServiceProviderEngine : ServiceProviderEngine
    {
        private readonly ILEmitResolverBuilder _expressionResolverBuilder;
        /// <summary>
        /// IL服务提供程序引擎-构造函数
        /// </summary>
        /// <param name="serviceDescriptors">服务描述集合</param>
        /// <param name="callback">服务提供程序引擎回调</param>
        public ILEmitServiceProviderEngine(IEnumerable<ServiceDescriptor> serviceDescriptors, IServiceProviderEngineCallback callback) : base(serviceDescriptors, callback)
        {
            _expressionResolverBuilder = new ILEmitResolverBuilder(RuntimeResolver, this, Root);
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
