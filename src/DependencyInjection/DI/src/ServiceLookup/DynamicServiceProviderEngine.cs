// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 动态服务提供程序引擎
    /// </summary>
    internal class DynamicServiceProviderEngine : CompiledServiceProviderEngine
    {
        /// <summary>
        /// 动态服务提供程序引擎-构造函数
        /// </summary>
        /// <param name="serviceDescriptors">服务描述集合</param>
        /// <param name="callback">服务提供程序引擎回调</param>
        public DynamicServiceProviderEngine(IEnumerable<ServiceDescriptor> serviceDescriptors, IServiceProviderEngineCallback callback) : base(serviceDescriptors, callback)
        {
        }
        /// <summary>
        /// 实现服务
        /// </summary>
        /// <param name="callSite">服务调用设置</param>
        /// <returns></returns>
        protected override Func<ServiceProviderEngineScope, object> RealizeService(ServiceCallSite callSite)
        {
            var callCount = 0;
            return scope =>
            {
                if (Interlocked.Increment(ref callCount) == 2)
                {
                    Task.Run(() => base.RealizeService(callSite));
                }
                return RuntimeResolver.Resolve(callSite, scope);
            };
        }
    }
}
