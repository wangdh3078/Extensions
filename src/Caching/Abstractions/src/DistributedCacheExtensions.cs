// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    ///分布式缓存扩展
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        ///使用指定的键设置指定高速缓存中的字节序列。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void Set(this IDistributedCache cache, string key, byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            cache.Set(key, value, new DistributedCacheEntryOptions());
        }

        /// <summary>
        /// 使用指定的键异步设置指定高速缓存中的字节序列。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>取消操作。</param>
        /// <returns>表示异步操作的任务.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetAsync(this IDistributedCache cache, string key, byte[] value, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return cache.SetAsync(key, value, new DistributedCacheEntryOptions(), token);
        }

        /// <summary>
        /// 使用指定的键设置指定高速缓存中的字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void SetString(this IDistributedCache cache, string key, string value)
        {
            cache.SetString(key, value, new DistributedCacheEntryOptions());
        }

        /// <summary>
        /// 使用指定的键设置指定高速缓存中的字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <param name="options">条目的缓存选项。</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void SetString(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            cache.Set(key, Encoding.UTF8.GetBytes(value), options);
        }

        /// <summary>
        ///使用指定的键异步设置指定高速缓存中的字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>取消操作。</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetStringAsync(this IDistributedCache cache, string key, string value, CancellationToken token = default(CancellationToken))
        {
            return cache.SetStringAsync(key, value, new DistributedCacheEntryOptions(), token);
        }

        /// <summary>
        /// 使用指定的键异步设置指定高速缓存中的字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="value">要存储在缓存中的数据。</param>
        /// <param name="options">条目的缓存选项</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>取消操作。</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetStringAsync(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options, token);
        }

        /// <summary>
        /// 从指定的缓存中获取具有指定键的字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <returns></returns>
        public static string GetString(this IDistributedCache cache, string key)
        {
            var data = cache.Get(key);
            if (data == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 使用指定的键从指定的缓存中异步获取字符串。
        /// </summary>
        /// <param name="cache">用于存储数据的缓存。</param>
        /// <param name="key">存储数据的键。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>取消操作。</param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken))
        {
            var data = await cache.GetAsync(key, token);
            if (data == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
    }
}
