// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Internal
{
    /// <summary>
    ///提供对正常系统时钟的访问。
    /// </summary>
    public class SystemClock : ISystemClock
    {
        /// <summary>
        /// 以UTC格式检索当前系统时间。
        /// </summary>
        public DateTimeOffset UtcNow
        {
            get
            {
                return DateTimeOffset.UtcNow;
            }
        }
    }
}
