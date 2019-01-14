// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 运行时服务提供程序引擎
    /// </summary>
    internal class RuntimeServiceProviderEngine : ServiceProviderEngine
    {
        /// <summary>
        /// 运行时服务提供程序引擎-构造函数
        /// </summary>
        /// <param name="serviceDescriptors">服务描述集合</param>
        /// <param name="callback">服务提供程序引擎回调</param>
        public RuntimeServiceProviderEngine(IEnumerable<ServiceDescriptor> serviceDescriptors, IServiceProviderEngineCallback callback) : base(serviceDescriptors, callback)
        {
        }
        /// <summary>
        /// 实现服务
        /// </summary>
        /// <param name="callSite">服务调用设置</param>
        /// <returns></returns>
        protected override Func<ServiceProviderEngineScope, object> RealizeService(ServiceCallSite callSite)
        {
            return scope =>
            {
                Func<ServiceProviderEngineScope, object> realizedService = p => RuntimeResolver.Resolve(callSite, p);

                RealizedServices[callSite.ServiceType] = realizedService;
                return realizedService(scope);
            };
        }
    }
}
