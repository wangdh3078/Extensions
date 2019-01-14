// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 服务提供程序引擎回调接口（检验validateScopes时使用）
    /// </summary>
    internal interface IServiceProviderEngineCallback
    {
        /// <summary>
        /// 创建时回调
        /// </summary>
        /// <param name="callSite"></param>
        void OnCreate(ServiceCallSite callSite);
        /// <summary>
        /// 解析式回调
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="scope">服务范围</param>
        void OnResolve(Type serviceType, IServiceScope scope);
    }
}
