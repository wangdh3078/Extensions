// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 缓存对象优先级
    /// </summary>
    public enum CacheItemPriority
    {
        /// <summary>
        /// 低
        /// </summary>
        Low,
        /// <summary>
        /// 一般
        /// </summary>
        Normal,
        /// <summary>
        /// 高
        /// </summary>
        High,
        /// <summary>
        /// 永不移除
        /// </summary>
        NeverRemove,
    }
}
