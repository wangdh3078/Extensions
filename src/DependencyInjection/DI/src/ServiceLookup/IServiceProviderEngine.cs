// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 服务提供程序引擎
    /// </summary>
    internal interface IServiceProviderEngine : IDisposable, IServiceProvider
    {
        /// <summary>
        /// 服务根节点
        /// </summary>
        IServiceScope RootScope { get; }
        /// <summary>
        /// 验证服务
        /// </summary>
        /// <param name="descriptor">服务描述</param>
        void ValidateService(ServiceDescriptor descriptor);
    }
}
