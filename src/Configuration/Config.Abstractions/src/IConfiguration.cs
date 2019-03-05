// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///表示一组键/值应用程序配置属性。
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 获取或设置配置值。
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>配置值</returns>
        string this[string key] { get; set; }

        /// <summary>
        ///获取具有指定键的配置子节。
        /// </summary>
        /// <param name="key">配置接点的键。</param>
        /// <returns></returns>
        /// <remarks>
        /// 此方法永远不会返回<c> null </c>。
        /// 如果找不到与指定键匹配的子节，则返回空<see cref ="IConfigurationSection"/>。"
        /// </remarks>
        IConfigurationSection GetSection(string key);

        /// <summary>
        /// 获取直接后代配置子节。
        /// </summary>
        /// <returns></returns>
        IEnumerable<IConfigurationSection> GetChildren();

        /// <summary>
        /// 返回<see cref ="IChangeToken"/>，可用于观察何时重新加载此配置。
        /// </summary>
        /// <returns></returns>
        IChangeToken GetReloadToken();
    }
}
