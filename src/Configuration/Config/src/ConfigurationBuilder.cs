// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 用于构建基于键/值的配置设置以在应用程序中使用。
    /// </summary>
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        /// <summary>
        /// 返回用于获取配置值的源。
        /// </summary>
        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();

        /// <summary>
        /// 获取一个键/值集合，可用于在<see cref ="IConfigurationBuilder"/>
        /// 和已注册的<see cref ="IConfigurationSource"/> s之间共享数据。
        /// </summary>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 添加新配置源。
        /// </summary>
        /// <param name="source">要添加的配置源。</param>
        /// <returns></returns>
        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        /// <summary>
        ///使用注册的源集中的键和值构建<see cref ="IConfiguration"/>
        /// <see cref="Sources"/>.
        /// </summary>
        /// <returns></returns>
        public IConfigurationRoot Build()
        {
            var providers = new List<IConfigurationProvider>();
            foreach (var source in Sources)
            {
                var provider = source.Build(this);
                providers.Add(provider);
            }
            return new ConfigurationRoot(providers);
        }
    }
}
