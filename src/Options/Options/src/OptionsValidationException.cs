// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 选项验证失败时抛出。
    /// </summary>
    public class OptionsValidationException : Exception
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="optionsName">失败的选项实例的名称。</param>
        /// <param name="optionsType">失败的选项类型。</param>
        /// <param name="failureMessages">验证失败消息。</param>
        public OptionsValidationException(string optionsName, Type optionsType, IEnumerable<string> failureMessages)
        {
            Failures = failureMessages ?? new List<string>();
            OptionsType = optionsType ?? throw new ArgumentNullException(nameof(optionsType));
            OptionsName = optionsName ?? throw new ArgumentNullException(nameof(optionsName));
        }

        /// <summary>
        /// 失败的选项实例的名称。
        /// </summary>
        public string OptionsName { get; }

        /// <summary>
        ///失败的选项类型。
        /// </summary>
        public Type OptionsType { get; }

        /// <summary>
        /// 验证失败。
        /// </summary>
        public IEnumerable<string> Failures { get; }
    }
}
