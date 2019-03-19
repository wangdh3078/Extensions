// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 表示配置TOptions类型的内容。
    /// 注意：这些都在所有<see cref ="IPostConfigureOptions {TOptions}"/>之前运行。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="action">注册的行为。</param>
        public ConfigureOptions(Action<TOptions> action)
        {
            Action = action;
        }

        /// <summary>
        /// 配置操作。
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// 如果名称匹配，则调用已注册的配置操作。
        /// </summary>
        /// <param name="options"></param>
        public virtual void Configure(TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Action?.Invoke(options);
        }
    }
}
