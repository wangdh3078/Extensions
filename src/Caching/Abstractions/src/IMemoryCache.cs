// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    ///表示其值未序列化的本地内存高速缓存。
    /// </summary>
    public interface IMemoryCache : IDisposable
    {
        /// <summary>
        /// 获取与此键关联的项（如果存在）。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <returns>如果找到缓存键，则为true。</returns>
        bool TryGetValue(object key, out object value);

        /// <summary>
        /// 创建或覆盖缓存中的条目。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>新创建的<see cref ="ICacheEntry"/>实例。</returns>
        ICacheEntry CreateEntry(object key);

        /// <summary>
        /// 删除与给定键关联的对象。
        /// </summary>
        /// <param name="key">缓存键</param>
        void Remove(object key);
    }
}
