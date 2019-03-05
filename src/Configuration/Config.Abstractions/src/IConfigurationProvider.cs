// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///提供应用程序的配置键/值。
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// 尝试获取指定键的配置值。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>如果找到指定键的值，则<c> True </c>，否则<c> false </c>。</returns>
        bool TryGet(string key, out string value);

        /// <summary>
        /// 设置指定键的配置值。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(string key, string value);

        /// <summary>
        ///如果此提供程序支持更改跟踪，则返回更改标记，否则返回null。
        /// </summary>
        /// <returns></returns>
        IChangeToken GetReloadToken();

        /// <summary>
        ///从此<see cref ="IConfigurationProvider"/>表示的源加载配置值。
        /// </summary>
        void Load();

        /// <summary>
        /// 基于此<see cref ="IConfigurationProvider"/>的数据
        /// 以及前面所有<see cref ="IConfigurationProvider"/>
        /// 返回的键集返回给定父路径的直接后代配置键。
        /// </summary>
        /// <param name="earlierKeys">前面的提供程序为相同的父路径返回的子键。</param>
        /// <param name="parentPath">父路径。</param>
        /// <returns>子节点所有键</returns>
        IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath);
    }
}
