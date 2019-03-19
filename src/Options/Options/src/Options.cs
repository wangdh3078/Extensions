// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class Options
    {
        /// <summary>
        /// 用于选项实例的默认名称：""。
        /// </summary>
        public static readonly string DefaultName = string.Empty;

        /// <summary>
        /// 在TOptions实例周围创建一个包装器，将其自身返回为IOptions。
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IOptions<TOptions> Create<TOptions>(TOptions options) where TOptions : class, new()
        {
            return new OptionsWrapper<TOptions>(options);
        }
    }
}
