// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// 为<see cref ="IDistributedCache"/>中的条目提供缓存选项。
    /// </summary>
    public class DistributedCacheEntryOptions
    {
        private DateTimeOffset? _absoluteExpiration;
        private TimeSpan? _absoluteExpirationRelativeToNow;
        private TimeSpan? _slidingExpiration;

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
        ///获取或设置相对于现在的绝对到期时间。
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
        ///获取或设置缓存在被删除之前可以处于非活动状态（例如，未访问）的时间。
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
    }
}
