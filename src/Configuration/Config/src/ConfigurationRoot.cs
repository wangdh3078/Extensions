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
    /// 配置的根节点。
    /// </summary>
    public class ConfigurationRoot : IConfigurationRoot
    {
        private readonly IList<IConfigurationProvider> _providers;
        private ConfigurationReloadToken _changeToken = new ConfigurationReloadToken();

        /// <summary>
        ///使用提供程序列表初始化配置根接点。
        /// </summary>
        /// <param name="providers">这个配置的<see cref ="IConfigurationProvider"/> s。</param>
        public ConfigurationRoot(IList<IConfigurationProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = providers;
            foreach (var p in providers)
            {
                p.Load();
                ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged());
            }
        }

        /// <summary>
        /// 配置的<see cref ="IConfigurationProvider"/> s。
        /// </summary>
        public IEnumerable<IConfigurationProvider> Providers => _providers;

        /// <summary>
        /// 获取或设置与配置键对应的值。
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>配置值</returns>
        public string this[string key]
        {
            get
            {
                foreach (var provider in _providers.Reverse())
                {
                    if (provider.TryGet(key, out var value))
                    {
                        return value;
                    }
                }

                return null;
            }
            set
            {
                if (!_providers.Any())
                {
                    throw new InvalidOperationException(Resources.Error_NoSources);
                }

                foreach (var provider in _providers)
                {
                    provider.Set(key, value);
                }
            }
        }

        /// <summary>
        /// 获取直接子节。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren() => this.GetChildrenImplementation(null);

        /// <summary>
        /// 返回<see cref ="IChangeToken"/>，可用于观察何时重新加载此配置。
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken() => _changeToken;

        /// <summary>
        ///获取具有指定键的配置子节。
        /// </summary>
        /// <param name="key">配置接点的键。</param>
        /// <returns></returns>
        /// <remarks>
        /// 此方法永远不会返回<c> null </c>。
        /// 如果找不到与指定键匹配的子节，则返回空<see cref ="IConfigurationSection"/>。"
        /// </remarks>
        public IConfigurationSection GetSection(string key) 
            => new ConfigurationSection(this, key);

        /// <summary>
        /// 强制从基础源重新加载配置值。
        /// </summary>
        public void Reload()
        {
            foreach (var provider in _providers)
            {
                provider.Load();
            }
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            var previousToken = Interlocked.Exchange(ref _changeToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }
    }
}
