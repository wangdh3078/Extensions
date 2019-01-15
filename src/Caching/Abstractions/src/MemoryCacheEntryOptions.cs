// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    ///表示应用于<see cref ="IMemoryCache"/>实例的条目的缓存选项.
    /// </summary>
    public class MemoryCacheEntryOptions
    {
        private DateTimeOffset? _absoluteExpiration;
        private TimeSpan? _absoluteExpirationRelativeToNow;
        private TimeSpan? _slidingExpiration;
        private long? _size;

        /// <summary>
        /// 获取或设置缓存条目的绝对过期日期。
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration
        {
            get
            {
                return _absoluteExpiration;
            }
            set
            {
                _absoluteExpiration = value;
            }
        }

        /// <summary>
        /// 获取或设置相对于now的绝对到期时间。
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get
            {
                return _absoluteExpirationRelativeToNow;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(AbsoluteExpirationRelativeToNow),
                        value,
                        "The relative expiration value must be positive.");
                }

                _absoluteExpirationRelativeToNow = value;
            }
        }

        /// <summary>
        /// 获取或设置缓存在被删除之前可以处于非活动状态（例如，未访问）的时间。
        /// 这不会将条目生存期延长到绝对到期时间（如果已设置）。
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(SlidingExpiration),
                        value,
                        "The sliding expiration value must be positive.");
                }
                _slidingExpiration = value;
            }
        }

        /// <summary>
        ///获取导致缓存条目到期的<see cref ="IChangeToken"/>实例。
        /// </summary>
        public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();

        /// <summary>
        ///获取或设置在从缓存中逐出缓存条目后将触发的回调。
        /// </summary>
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }
            = new List<PostEvictionCallbackRegistration>();

        /// <summary>
        ///获取或设置在内存压力触发清理期间将缓存条目保留在缓存中的优先级。
        ///默认值为<see cref ="CacheItemPriority.Normal"/>。
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

        /// <summary>
        /// 获取或设置缓存条目值的大小。
        /// </summary>
        public long? Size
        {
            get => _size;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be non-negative.");
                }

                _size = value;
            }
        }
    }
}
