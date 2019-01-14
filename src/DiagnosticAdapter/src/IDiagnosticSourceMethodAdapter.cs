// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Extensions.DiagnosticAdapter
{
    /// <summary>
    /// 诊断源方法适配器
    /// </summary>
    public interface IDiagnosticSourceMethodAdapter
    {
        /// <summary>
        /// 适配器
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="inputType">输入类型</param>
        /// <returns></returns>
        Func<object, object, bool> Adapt(MethodInfo method, Type inputType);
    }
}
