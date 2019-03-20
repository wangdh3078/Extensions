// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 执行<see cref ="IValidateOptions {TOptions}"/>
    /// </summary>
    /// <typeparam name="TOptions">正在验证的实例。</typeparam>
    public class ValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="name">选项名称。</param>
        /// <param name="validation">验证操作。</param>
        /// <param name="failureMessage">验证失败时返回的错误。</param>
        public ValidateOptions(string name, Func<TOptions, bool> validation, string failureMessage)
        {
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
        }

        /// <summary>
        /// 选项名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 验证操作。
        /// </summary>
        public Func<TOptions, bool> Validation { get; }

        /// <summary>
        /// 验证失败时返回的错误。
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        ///验证特定的命名选项实例（或者当name为null时验证所有实例）。
        /// </summary>
        /// <param name="name">要验证的选项实例的名称.</param>
        /// <param name="options">选项实例。</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                if ((Validation?.Invoke(options)).Value)
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }

            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }
    }
}
