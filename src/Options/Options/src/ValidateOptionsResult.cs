// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 选项验证的结果。
    /// </summary>
    public class ValidateOptionsResult
    {
        /// <summary>
        /// 由于名称不匹配而跳过验证的结果。
        /// </summary>
        public static readonly ValidateOptionsResult Skip = new ValidateOptionsResult() { Skipped = true };

        /// <summary>
        /// 验证成功。
        /// </summary>
        public static readonly ValidateOptionsResult Success = new ValidateOptionsResult() { Succeeded = true };

        /// <summary>
        ///如果验证成功，则为True。
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// 如果未运行验证，则为True。
        /// </summary>
        public bool Skipped { get; protected set; }

        /// <summary>
        /// 如果验证失败则为True。
        /// </summary>
        public bool Failed { get; protected set; }

        /// <summary>
        /// 用于描述验证失败的原因。
        /// </summary>
        public string FailureMessage { get; protected set; }

        /// <summary>
        /// 返回失败结果。
        /// </summary>
        /// <param name="failureMessage">失败的原因。</param>
        /// <returns>失败的结果。</returns>
        public static ValidateOptionsResult Fail(string failureMessage)
            => new ValidateOptionsResult { Failed = true, FailureMessage = failureMessage };
    }
}
