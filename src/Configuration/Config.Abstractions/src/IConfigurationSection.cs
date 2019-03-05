// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 表示应用程序配置值的一部分。
    /// </summary>
    public interface IConfigurationSection : IConfiguration
    {
        /// <summary>
        /// 获取此部分在其父级中占用的键。
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 在<see cref ="IConfiguration"/>中获取此部分的完整路径。
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 获取或设置节值。
        /// </summary>
        string Value { get; set; }
    }
}
