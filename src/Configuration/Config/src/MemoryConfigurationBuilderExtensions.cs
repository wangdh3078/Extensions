// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Memory;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// IConfigurationBuilder的MemoryConfigurationProvider扩展方法。
    /// </summary>
    public static class MemoryConfigurationBuilderExtensions
    {
        /// <summary>
        /// 将内存配置提供程序添加到<paramref name ="configurationBuilder"/>。
        /// </summary>
        /// <param name="configurationBuilder">要添加的<see cref ="IConfigurationBuilder"/>。</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.Add(new MemoryConfigurationSource());
            return configurationBuilder;
        }

        /// <summary>
        /// 将内存配置提供程序添加到<paramref name ="configurationBuilder"/>。
        /// </summary>
        /// <param name="configurationBuilder">要添加的<see cref ="IConfigurationBuilder"/>。</param>
        /// <param name="initialData">要添加到内存配置提供程序的数据。</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInMemoryCollection(
            this IConfigurationBuilder configurationBuilder,
            IEnumerable<KeyValuePair<string, string>> initialData)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.Add(new MemoryConfigurationSource { InitialData = initialData });
            return configurationBuilder;
        }
    }
}
