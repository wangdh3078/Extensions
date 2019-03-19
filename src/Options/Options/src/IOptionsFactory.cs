// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于创建TOptions实例。
    /// </summary>
    /// <typeparam name="TOptions">要求的选项类型。</typeparam>
    public interface IOptionsFactory<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// 返回具有给定名称的已配置TOptions实例。
        /// </summary>
        TOptions Create(string name);
    }
}
