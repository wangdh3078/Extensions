// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DiagnosticAdapter.Internal
{
    /// <summary>
    /// 代理操作无效异常
    /// </summary>
    public class InvalidProxyOperationException : InvalidOperationException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public InvalidProxyOperationException(string message) : base(message)
        {
        }
    }
}
