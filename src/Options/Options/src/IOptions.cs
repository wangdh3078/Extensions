// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于检索已配置的TOptions实例。
    /// </summary>
    /// <typeparam name="TOptions">要求的选项类型。</typeparam>
    public interface IOptions<out TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// 默认配置的TOptions实例
        /// </summary>
        TOptions Value { get; }
    }
}
