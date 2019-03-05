// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///配置类的扩展方法
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 添加新配置源。
        /// </summary>
        /// <param name="builder">要添加的<see cref ="IConfigurationBuilder"/>。</param>
        /// <param name="configureSource">配置源</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder Add<TSource>(this IConfigurationBuilder builder, Action<TSource> configureSource) where TSource : IConfigurationSource, new()
        {
            var source = new TSource();
            configureSource?.Invoke(source);
            return builder.Add(source);
        }

        /// <summary>
        /// GetSection("ConnectionStrings")[name]的简写.
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="name">配置键</param>
        /// <returns></returns>
        public static string GetConnectionString(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("ConnectionStrings")?[name];
        }

        /// <summary>
        /// 获取<see cref ="IConfiguration"/>中的键值对的枚举
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration) => configuration.AsEnumerable(makePathsRelative: false);

        /// <summary>
        /// 获取<see cref ="IConfiguration"/>中的键值对的枚举
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="makePathsRelative">如果为true，则返回的子键将从前面剪切当前配置的Path。</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration, bool makePathsRelative)
        {
            var stack = new Stack<IConfiguration>();
            stack.Push(configuration);
            var rootSection = configuration as IConfigurationSection;
            var prefixLength = (makePathsRelative && rootSection != null) ? rootSection.Path.Length + 1 : 0;
            while (stack.Count > 0)
            {
                var config = stack.Pop();
                // Don't include the sections value if we are removing paths, since it will be an empty key
                if (config is IConfigurationSection section && (!makePathsRelative || config != configuration))
                {
                    yield return new KeyValuePair<string, string>(section.Path.Substring(prefixLength), section.Value);
                }
                foreach (var child in config.GetChildren())
                {
                    stack.Push(child);
                }
            }
        }

        /// <summary>
        ///确定该部分是否具有<see cref ="IConfigurationSection.Value"/>或具有子项
        /// </summary>
        public static bool Exists(this IConfigurationSection section)
        {
            if (section == null)
            {
                return false;
            }
            return section.Value != null || section.GetChildren().Any();
        }
    }
}
