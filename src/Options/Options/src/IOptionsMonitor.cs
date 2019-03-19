// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于TOptions实例更改时的通知。
    /// </summary>
    /// <typeparam name="TOptions">选项类型。</typeparam>
    public interface IOptionsMonitor<out TOptions>
    {
        /// <summary>
        /// 使用<see cref ="Options.DefaultName"/>返回当前的TOptions实例。
        /// </summary>
        TOptions CurrentValue { get; }

        /// <summary>
        /// 返回具有给定名称的已配置TOptions实例。
        /// </summary>
        TOptions Get(string name);

        /// <summary>
        /// 只要命名的TOptions发生更改，就会注册一个侦听器。
        /// </summary>
        /// <param name="listener">TOptions更改时要调用的操作。</param>
        /// <returns>An IDisposable which should be disposed to stop listening for changes.</returns>
        IDisposable OnChange(Action<TOptions, string> listener);
    }
}
