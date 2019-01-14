// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务生命周期
    /// </summary>
    public enum ServiceLifetime
    {
        /// <summary>
        /// 指定将创建单个服务实例。
        /// </summary>
        Singleton,
        /// <summary>
        /// 指定将为每个范围创建新的服务实例。
        /// </summary>
        /// <remarks>
        ///在ASP.NET Core应用程序中，围绕每个服务器请求创建一个范围。
        /// </remarks>
        Scoped,
        /// <summary>
        ///指定每次请求时都会创建新的服务实例。
        /// </summary>
        Transient
    }
}
