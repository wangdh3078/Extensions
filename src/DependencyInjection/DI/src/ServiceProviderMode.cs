// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务提供类型
    /// </summary>
    internal enum ServiceProviderMode
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 动态
        /// </summary>
        Dynamic,
        /// <summary>
        /// 运行时
        /// </summary>
        Runtime,
        /// <summary>
        /// 表达式
        /// </summary>
        Expressions,
        /// <summary>
        /// IL
        /// </summary>
        ILEmit
    }
}
