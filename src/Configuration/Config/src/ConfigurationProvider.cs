// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 用于实现<see cref ="IConfigurationProvider"/>的基本辅助类
    /// </summary>
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        /// <summary>
        ///构造函数
        /// </summary>
        protected ConfigurationProvider()
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 此提供程序的配置键值对。
        /// </summary>
        protected IDictionary<string, string> Data { get; set; }

        /// <summary>
        /// 尝试使用给定键查找值，如果找到，则返回true，否则返回false。
        /// </summary>
        /// <param name="key">查找的键。</param>
        /// <param name="value">通过建找到的值</param>
        /// <returns>如果key有值，则返回true，否则返回false。</returns>
        public virtual bool TryGet(string key, out string value)
            => Data.TryGetValue(key, out value);

        /// <summary>
        /// 设置指定键的配置值。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public virtual void Set(string key, string value)
            => Data[key] = value;

        /// <summary>
        /// 加载（或重新加载）此提供程序的数据。
        /// </summary>
        public virtual void Load()
        { }

        /// <summary>
        ///返回此提供程序具有的键列表。
        /// </summary>
        /// <param name="earlierKeys">前面的提供程序为相同的父路径返回的子键。</param>
        /// <param name="parentPath">父路径。</param>
        /// <returns>The list of keys for this provider.</returns>
        public virtual IEnumerable<string> GetChildKeys(
            IEnumerable<string> earlierKeys,
            string parentPath)
        {
            var prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

            return Data
                .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kv => Segment(kv.Key, prefix.Length))
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefixLength"></param>
        /// <returns></returns>
        private static string Segment(string key, int prefixLength)
        {
            var indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
            return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        }

        /// <summary>
        /// 返回<see cref ="IChangeToken"/>，可用于在重新加载此提供程序时进行侦听。
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        /// <summary>
        /// 触发重新加载更改令牌并创建一个新令牌。
        /// </summary>
        protected void OnReload()
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        /// <summary>
        /// 生成表示此提供程序名称和相关详细信息的字符串。
        /// </summary>
        /// <returns> 配置名称。 </returns>
        public override string ToString() => $"{GetType().Name}";
    }
}
