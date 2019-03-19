// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.Memory
{
    /// <summary>
    /// <see cref ="IConfigurationProvider"/>的内存实现
    /// </summary>
    public class MemoryConfigurationProvider : ConfigurationProvider, IEnumerable<KeyValuePair<string, string>>
    {
        private readonly MemoryConfigurationSource _source;

        /// <summary>
        /// 从源初始化新实例。
        /// </summary>
        /// <param name="source">源设置。</param>
        public MemoryConfigurationProvider(MemoryConfigurationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _source = source;

            if (_source.InitialData != null)
            {
                foreach (var pair in _source.InitialData)
                {
                    Data.Add(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// 添加新的键和值对。
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="value">配置值</param>
        public void Add(string key, string value)
        {
            Data.Add(key, value);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
