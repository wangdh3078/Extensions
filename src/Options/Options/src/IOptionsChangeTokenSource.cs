// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    ///用于获取用于跟踪选项更改的IChangeTokens。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptionsChangeTokenSource<out TOptions>
    {
        /// <summary>
        ///返回IChangeToken，可用于注册更改通知回调。
        /// </summary>
        /// <returns></returns>
        IChangeToken GetChangeToken();

        /// <summary>
        /// 要更改的选项实例的名称。
        /// </summary>
        string Name { get; }
    }
}
