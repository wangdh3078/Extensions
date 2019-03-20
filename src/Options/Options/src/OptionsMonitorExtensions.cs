// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    ///IOptionsMonitor的扩展方法。
    /// </summary>
    public static class OptionsMonitorExtensions
    {
        /// <summary>
        ///在TOptions更改时注册要调用的侦听器。
        /// </summary>
        /// <param name="monitor">The IOptionsMonitor.</param>
        /// <param name="listener">TOptions更改时要调用的操作。</param>
        /// <returns>An IDisposable which should be disposed to stop listening for changes.</returns>
        public static IDisposable OnChange<TOptions>(this IOptionsMonitor<TOptions> monitor, Action<TOptions> listener)
            => monitor.OnChange((o, _) => listener(o));
    }
}
