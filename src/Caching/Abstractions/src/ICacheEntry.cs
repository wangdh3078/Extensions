// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 表示<see cref ="IMemoryCache"/>实现中的实体。
    /// </summary>
    public interface ICacheEntry : IDisposable
    {
        /// <summary>
        ///获取缓存的键。
        /// </summary>
        object Key { get; }

        /// <summary>
        /// 获取或设置缓存的值。
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///获取或设置缓存的绝对过期日期。
        /// </summary>
        DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        ///获取或设置相对于现在的绝对到期时间。
        /// </summary>
        TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        /// <summary>
        /// 获取或设置缓存在被删除之前可以处于非活动状态（例如，未访问）的时间。
        /// 这不会将条目生存期延长到绝对到期时间（如果已设置）。
        /// </summary>
        TimeSpan? SlidingExpiration { get; set; }

        /// <summary>
        ///获取导致缓存到期的<see cref ="IChangeToken"/>实例。
        /// </summary>
        IList<IChangeToken> ExpirationTokens { get; }

        /// <summary>
        /// 获取或设置在从缓存中删除缓存条目后将触发的回调。
        /// </summary>
        IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }

        /// <summary>
        /// 获取或设置在清理期间将缓存条目保留在缓存中的优先级。
        /// 默认值为<see cref ="CacheItemPriority.Normal"/>。
        /// </summary>
        CacheItemPriority Priority { get; set; }

        /// <summary>
        ///缓存大小
        /// </summary>
        long? Size { get; set; }
    }
}
