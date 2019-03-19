// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 表示配置TOptions类型的内容。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigureNamedOptions<in TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// 调用以配置TOptions实例。
        /// </summary>
        /// <param name="name">正在配置的选项实例的名称。</param>
        /// <param name="options">要配置的选项实例。</param>
        void Configure(string name, TOptions options);
    }
}
