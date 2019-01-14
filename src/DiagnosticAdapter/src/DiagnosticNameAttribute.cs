// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DiagnosticAdapter
{
    /// <summary>
    /// 诊断名称属性
    /// </summary>
    public class DiagnosticNameAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">诊断名称</param>
        public DiagnosticNameAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 诊断名称
        /// </summary>
        public string Name { get; }
    }
}
