// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// 分布式缓存条目扩展
    /// </summary>
    public static class DistributedCacheEntryExtensions
    {
        /// <summary>
        ///设置相对于现在的绝对到期时间。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="relative"></param>
        public static DistributedCacheEntryOptions SetAbsoluteExpiration(
            this DistributedCacheEntryOptions options,
            TimeSpan relative)
        {
            options.AbsoluteExpirationRelativeToNow = relative;
            return options;
        }

        /// <summary>
        /// 设置缓存条目的绝对到期日期。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="absolute"></param>
        public static DistributedCacheEntryOptions SetAbsoluteExpiration(
            this DistributedCacheEntryOptions options,
            DateTimeOffset absolute)
        {
            options.AbsoluteExpiration = absolute;
            return options;
        }

        /// <summary>
        /// 获取或设置缓存在被删除之前可以处于非活动状态（例如，未访问）的时间。
        /// 这不会将条目生存期延长到绝对到期时间（如果已设置）。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="offset"></param>
        public static DistributedCacheEntryOptions SetSlidingExpiration(
            this DistributedCacheEntryOptions options,
            TimeSpan offset)
        {
            options.SlidingExpiration = offset;
            return options;
        }
    }
}
