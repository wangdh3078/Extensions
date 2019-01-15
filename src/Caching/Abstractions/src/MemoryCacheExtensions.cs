// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 内存缓存扩展
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 根据指定键获取缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(this IMemoryCache cache, object key)
        {
            cache.TryGetValue(key, out object value);
            return value;
        }
        /// <summary>
        /// 根据指定键获取缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TItem Get<TItem>(this IMemoryCache cache, object key)
        {
            return (TItem)(cache.Get(key) ?? default(TItem));
        }
        /// <summary>
        /// 根据指定键获取缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<TItem>(this IMemoryCache cache, object key, out TItem value)
        {
            if (cache.TryGetValue(key, out object result))
            {
                if (result is TItem item)
                {
                    value = item;
                    return true;
                }
            }

            value = default;
            return false;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value)
        {
            var entry = cache.CreateEntry(key);
            entry.Value = value;
            entry.Dispose();

            return value;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            var entry = cache.CreateEntry(key);
            entry.AbsoluteExpiration = absoluteExpiration;
            entry.Value = value;
            entry.Dispose();

            return value;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <returns></returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            var entry = cache.CreateEntry(key);
            entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            entry.Value = value;
            entry.Dispose();

            return value;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationToken"></param>
        /// <returns></returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, IChangeToken expirationToken)
        {
            var entry = cache.CreateEntry(key);
            entry.AddExpirationToken(expirationToken);
            entry.Value = value;
            entry.Dispose();

            return value;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, MemoryCacheEntryOptions options)
        {
            using (var entry = cache.CreateEntry(key))
            {
                if (options != null)
                {
                    entry.SetOptions(options);
                }

                entry.Value = value;
            }

            return value;
        }
        /// <summary>
        /// 获取或设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static TItem GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, TItem> factory)
        {
            if (!cache.TryGetValue(key, out object result))
            {
                var entry = cache.CreateEntry(key);
                result = factory(entry);
                entry.SetValue(result);
                // need to manually call dispose instead of having a using
                // in case the factory passed in throws, in which case we
                // do not want to add the entry to the cache
                entry.Dispose();
            }

            return (TItem)result;
        }
        /// <summary>
        /// 获取或设置缓存
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            if (!cache.TryGetValue(key, out object result))
            {
                var entry = cache.CreateEntry(key);
                result = await factory(entry);
                entry.SetValue(result);
                // need to manually call dispose instead of having a using
                // in case the factory passed in throws, in which case we
                // do not want to add the entry to the cache
                entry.Dispose();
            }

            return (TItem)result;
        }
    }
}
