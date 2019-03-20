// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于缓存TOptions实例。
    /// </summary>
    /// <typeparam name="TOptions">要求的选项类型。</typeparam>
    public class OptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
    {
        private readonly ConcurrentDictionary<string, Lazy<TOptions>> _cache = new ConcurrentDictionary<string, Lazy<TOptions>>(StringComparer.Ordinal);

        /// <summary>
        /// 清除缓存中的所有选项实例。
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// 获取命名选项实例，或添加使用createOptions创建的新实例。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <param name="createOptions">用于创建新实例的func。</param>
        /// <returns>选项实例。</returns>
        public virtual TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            if (createOptions == null)
            {
                throw new ArgumentNullException(nameof(createOptions));
            }
            name = name ?? Options.DefaultName;
            return _cache.GetOrAdd(name, new Lazy<TOptions>(createOptions)).Value;
        }

        /// <summary>
        /// 尝试向缓存添加新选项，如果名称已存在则返回false。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <param name="options">选项实例。</param>
        /// <returns></returns>
        public virtual bool TryAdd(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            name = name ?? Options.DefaultName;
            return _cache.TryAdd(name, new Lazy<TOptions>(() => options));
        }

        /// <summary>
        ///尝试删除选项实例。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <returns></returns>
        public virtual bool TryRemove(string name)
        {
            name = name ?? Options.DefaultName;
            return _cache.TryRemove(name, out var ignored);
        }
    }
}
