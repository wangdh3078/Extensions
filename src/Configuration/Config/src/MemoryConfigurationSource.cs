// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.Memory
{
    /// <summary>
    /// 将内存数据表示为<see cref ="IConfigurationSource"/>。
    /// </summary>
    public class MemoryConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// 初始键值配置对。
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> InitialData { get; set; }

        /// <summary>
        /// 为此源构建<see cref ="MemoryConfigurationProvider"/>。
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="MemoryConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MemoryConfigurationProvider(this);
        }
    }
}
