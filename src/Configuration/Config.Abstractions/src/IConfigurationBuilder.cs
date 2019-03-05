// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///用于构建应用程序配置的类型
    /// </summary>
    public interface IConfigurationBuilder
    {
        /// <summary>
        /// 获取一个键/值集合，可用于在<see cref ="IConfigurationBuilder"/>
        /// 和已注册的<see cref ="IConfigurationSource"/> s之间共享数据。
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// 获取用于获取配置值的源
        /// </summary>
        IList<IConfigurationSource> Sources { get; }

        /// <summary>
        /// 添加新配置源。
        /// </summary>
        /// <param name="source">要添加的配置源。</param>
        /// <returns></returns>
        IConfigurationBuilder Add(IConfigurationSource source);

        /// <summary>
        ///使用注册的源集中的键和值构建<see cref ="IConfiguration"/>
        /// <see cref="Sources"/>.
        /// </summary>
        /// <returns></returns>
        IConfigurationRoot Build();
    }
}
