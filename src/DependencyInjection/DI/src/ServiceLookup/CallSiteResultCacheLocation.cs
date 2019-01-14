// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 调用结果缓存位置
    /// </summary>
    internal enum CallSiteResultCacheLocation
    {
        /// <summary>
        /// 根节点
        /// </summary>
        Root,
        /// <summary>
        /// 服务范围内
        /// </summary>
        Scope,
        /// <summary>
        /// 回收
        /// </summary>
        Dispose,
        /// <summary>
        /// 不缓存
        /// </summary>
        None
    }
}
