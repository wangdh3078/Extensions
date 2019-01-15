// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 内存缓存设置扩展
    /// </summary>
    public static class MemoryCacheEntryExtensions
    {
        /// <summary>
        /// 设置在内存压力清除期间将缓存条目保留在缓存中的优先级。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="priority"></param>
        public static MemoryCacheEntryOptions SetPriority(
            this MemoryCacheEntryOptions options,
            CacheItemPriority priority)
        {
            options.Priority = priority;
            return options;
        }

        /// <summary>
        ///设置缓存条目值的大小。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="size"></param>
        public static MemoryCacheEntryOptions SetSize(
            this MemoryCacheEntryOptions options,
            long size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, $"{nameof(size)} must be non-negative.");
            }

            options.Size = size;
            return options;
        }

        /// <summary>
        ///如果给定的<see cref ="IChangeToken"/>到期，则使缓存条目到期。
        /// </summary>
        /// <param name="options">The <see cref="MemoryCacheEntryOptions"/>.</param>
        /// <param name="expirationToken">The <see cref="IChangeToken"/> that causes the cache entry to expire.</param>
        public static MemoryCacheEntryOptions AddExpirationToken(
            this MemoryCacheEntryOptions options,
            IChangeToken expirationToken)
        {
            if (expirationToken == null)
            {
                throw new ArgumentNullException(nameof(expirationToken));
            }

            options.ExpirationTokens.Add(expirationToken);
            return options;
        }

        /// <summary>
        /// 设置相对于现在的绝对到期时间。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="relative"></param>
        public static MemoryCacheEntryOptions SetAbsoluteExpiration(
            this MemoryCacheEntryOptions options,
            TimeSpan relative)
        {
            options.AbsoluteExpirationRelativeToNow = relative;
            return options;
        }

        /// <summary>
        /// 设置缓存条目的绝对到期日期。ssssssssssssssssssssssssssssssssssssssssssssssssss
        /// </summary>
        /// <param name="options"></param>
        /// <param name="absolute"></param>
        public static MemoryCacheEntryOptions SetAbsoluteExpiration(
            this MemoryCacheEntryOptions options,
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
        public static MemoryCacheEntryOptions SetSlidingExpiration(
            this MemoryCacheEntryOptions options,
            TimeSpan offset)
        {
            options.SlidingExpiration = offset;
            return options;
        }

        /// <summary>
        /// 在从缓存中逐出缓存条目后，将触发给定的回调。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="callback"></param>
        public static MemoryCacheEntryOptions RegisterPostEvictionCallback(
            this MemoryCacheEntryOptions options,
            PostEvictionDelegate callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return options.RegisterPostEvictionCallback(callback, state: null);
        }

        /// <summary>
        /// 在从缓存中逐出缓存条目后，将触发给定的回调。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public static MemoryCacheEntryOptions RegisterPostEvictionCallback(
            this MemoryCacheEntryOptions options,
            PostEvictionDelegate callback,
            object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = callback,
                State = state
            });
            return options;
        }
    }
}
