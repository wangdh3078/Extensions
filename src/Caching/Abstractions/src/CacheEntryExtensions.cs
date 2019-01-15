// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 缓存实体扩展
    /// </summary>
    public static class CacheEntryExtensions
    {
        /// <summary>
        /// 设置在清除缓存期间将缓存条目保留在缓存中的优先级。
        /// </summary>
        /// <param name="entry">缓存实体</param>
        /// <param name="priority">缓存优先级</param>
        public static ICacheEntry SetPriority(
            this ICacheEntry entry,
            CacheItemPriority priority)
        {
            entry.Priority = priority;
            return entry;
        }

        /// <summary>
        /// 如果给定的<see cref ="IChangeToken"/>到期，则使缓存条目到期。
        /// </summary>
        /// <param name="entry">缓存实体</param>
        /// <param name="expirationToken"><see cref ="IChangeToken"/>导致缓存条目过期。</param>
        public static ICacheEntry AddExpirationToken(
            this ICacheEntry entry,
            IChangeToken expirationToken)
        {
            if (expirationToken == null)
            {
                throw new ArgumentNullException(nameof(expirationToken));
            }

            entry.ExpirationTokens.Add(expirationToken);
            return entry;
        }

        /// <summary>
        /// 设置相对于现在的绝对到期时间。
        /// </summary>
        /// <param name="entry">缓存实体</param>
        /// <param name="relative">相对时间</param>
        public static ICacheEntry SetAbsoluteExpiration(
            this ICacheEntry entry,
            TimeSpan relative)
        {
            entry.AbsoluteExpirationRelativeToNow = relative;
            return entry;
        }

        /// <summary>
        ///设置缓存条目的绝对到期日期。
        /// </summary>
        /// <param name="entry">缓存实体</param>
        /// <param name="absolute">到期时间</param>
        public static ICacheEntry SetAbsoluteExpiration(
            this ICacheEntry entry,
            DateTimeOffset absolute)
        {
            entry.AbsoluteExpiration = absolute;
            return entry;
        }

        /// <summary>
        ///  获取或设置缓存在被删除之前可以处于非活动状态（例如，未访问）的时间。
        /// 这不会将条目生存期延长到绝对到期时间（如果已设置）。
        /// </summary>
        /// <param name="entry">缓存实体</param>
        /// <param name="offset"></param>
        public static ICacheEntry SetSlidingExpiration(
            this ICacheEntry entry,
            TimeSpan offset)
        {
            entry.SlidingExpiration = offset;
            return entry;
        }

        /// <summary>
        /// 在从缓存中逐出缓存条目后，将触发给定的回调。
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="callback"></param>
        public static ICacheEntry RegisterPostEvictionCallback(
            this ICacheEntry entry,
            PostEvictionDelegate callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return entry.RegisterPostEvictionCallback(callback, state: null);
        }

        /// <summary>
        /// 在从缓存中逐出缓存条目后，将触发给定的回调。
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public static ICacheEntry RegisterPostEvictionCallback(
            this ICacheEntry entry,
            PostEvictionDelegate callback,
            object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = callback,
                State = state
            });
            return entry;
        }

        /// <summary>
        /// 设置缓存条目的值。
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static ICacheEntry SetValue(
            this ICacheEntry entry,
            object value)
        {
            entry.Value = value;
            return entry;
        }

        /// <summary>
        /// 设置缓存条目值的大小。
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="size"></param>
        public static ICacheEntry SetSize(
            this ICacheEntry entry,
            long size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, $"{nameof(size)} must be non-negative.");
            }

            entry.Size = size;
            return entry;
        }

        /// <summary>
        /// 将现有<see cref ="MemoryCacheEntryOptions"/>的值应用于该条目。
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="options"></param>
        public static ICacheEntry SetOptions(this ICacheEntry entry, MemoryCacheEntryOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            entry.AbsoluteExpiration = options.AbsoluteExpiration;
            entry.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = options.SlidingExpiration;
            entry.Priority = options.Priority;
            entry.Size = options.Size;

            foreach (var expirationToken in options.ExpirationTokens)
            {
                entry.AddExpirationToken(expirationToken);
            }

            foreach (var postEvictionCallback in options.PostEvictionCallbacks)
            {
                entry.RegisterPostEvictionCallback(postEvictionCallback.EvictionCallback, postEvictionCallback.State);
            }

            return entry;
        }
    }
}
