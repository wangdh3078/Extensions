// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///表示应用程序配置值的一部分。
    /// </summary>
    public class ConfigurationSection : IConfigurationSection
    {
        private readonly IConfigurationRoot _root;
        private readonly string _path;
        private string _key;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="root">配置根节点</param>
        /// <param name="path">本节点的路径。</param>
        public ConfigurationSection(IConfigurationRoot root, string path)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _root = root;
            _path = path;
        }

        /// <summary>
        ///从<see cref ="IConfigurationRoot"/>获取此部分的完整路径。
        /// </summary>
        public string Path => _path;

        /// <summary>
        /// 获取此部分在其父级中占用的键。
        /// </summary>
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    // Key is calculated lazily as last portion of Path
                    _key = ConfigurationPath.GetSectionKey(_path);
                }
                return _key;
            }
        }

        /// <summary>
        ///获取或设置节值。
        /// </summary>
        public string Value
        {
            get
            {
                return _root[Path];
            }
            set
            {
                _root[Path] = value;
            }
        }

        /// <summary>
        /// 获取或设置与配置键对应的值。
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return _root[ConfigurationPath.Combine(Path, key)];
            }

            set
            {
                _root[ConfigurationPath.Combine(Path, key)] = value;
            }
        }

        /// <summary>
        ///获取具有指定键的配置子节。
        /// </summary>
        /// <param name="key">配置接点的键。</param>
        /// <returns></returns>
        /// <remarks>
        /// 此方法永远不会返回<c> null </c>。
        /// 如果找不到与指定键匹配的子节，则返回空<see cref ="IConfigurationSection"/>。"
        /// </remarks>
        public IConfigurationSection GetSection(string key) => _root.GetSection(ConfigurationPath.Combine(Path, key));

        /// <summary>
        /// 获取直接后代配置子节。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren() => _root.GetChildrenImplementation(Path);

        /// <summary>
        /// 返回<see cref ="IChangeToken"/>，可用于观察何时重新加载此配置。
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken() => _root.GetReloadToken();
    }
}
