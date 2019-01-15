// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Internal
{
    /// <summary>
    /// 抽象系统时钟以方便测试。
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// 以UTC格式检索当前系统时间。
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
