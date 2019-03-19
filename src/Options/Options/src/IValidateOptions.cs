// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于验证选项的接口。
    /// </summary>
    /// <typeparam name="TOptions">要验证的选项类型。</typeparam>
    public interface IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// 验证特定的命名选项实例（或者当name为null时验证所有实例）。
        /// </summary>
        /// <param name="name">要验证的选项实例的名称。</param>
        /// <param name="options">选项实例。</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        ValidateOptionsResult Validate(string name, TOptions options);
    }
}
