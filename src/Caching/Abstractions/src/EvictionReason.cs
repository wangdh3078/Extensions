// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 删除原因
    /// </summary>
    public enum EvictionReason
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 手动
        /// </summary>
        Removed,

        /// <summary>
        /// 覆盖
        /// </summary>
        Replaced,

        /// <summary>
        /// 时间到
        /// </summary>
        Expired,

        /// <summary>
        /// 事件
        /// </summary>
        TokenExpired,

        /// <summary>
        /// 溢出
        /// </summary>
        Capacity,
    }
}
