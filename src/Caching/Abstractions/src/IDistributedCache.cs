// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// 表示序列化值的分布式缓存。
    /// </summary>
    public interface IDistributedCache
    {
        /// <summary>
        ///获取具有给定键的值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        /// <returns>找到的值或null。</returns>
        byte[] Get(string key);

        /// <summary>
        ///获取具有给定键的值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>用于传播应该取消操作的通知。</param>
        /// <returns></returns>
        Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 使用给定键设置值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        /// <param name="value">要在缓存中设置的值。</param>
        /// <param name="options">值的缓存选项。</param>
        void Set(string key, byte[] value, DistributedCacheEntryOptions options);

        /// <summary>
        /// 使用给定键设置值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        /// <param name="value">要在缓存中设置的值。</param>
        /// <param name="options">值的缓存选项。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>用于传播应该取消操作的通知。</param>
        /// <returns></returns>
        Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 根据缓存中的键刷新缓存中的值，重置其滑动到期超时（如果有）。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        void Refresh(string key);

        /// <summary>
        ///根据缓存中的键刷新缓存中的值，重置其滑动到期超时（如果有）。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>用于传播应该取消操作的通知。</param>
        /// <returns>.</returns>
        Task RefreshAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 使用给定键删除值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        void Remove(string key);

        /// <summary>
        /// 使用给定键删除值。
        /// </summary>
        /// <param name="key">标识所请求值的字符串。</param>
        /// <param name="token">可选的。 <see cref ="CancellationToken"/>用于传播应该取消操作的通知。</param>
        /// <returns></returns>
        Task RemoveAsync(string key, CancellationToken token = default(CancellationToken));
    }
}
