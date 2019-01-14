// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.DiagnosticAdapter.Infrastructure;
using Microsoft.Extensions.DiagnosticAdapter.Internal;

namespace Microsoft.Extensions.DiagnosticAdapter
{
    /// <summary>
    /// 诊断源方法适配器
    /// </summary>
    public class ProxyDiagnosticSourceMethodAdapter : IDiagnosticSourceMethodAdapter
    {
        /// <summary>
        /// 代理工厂
        /// </summary>
        private readonly IProxyFactory _factory = new ProxyFactory();
        /// <summary>
        /// 适配器
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="inputType">输入类型</param>
        /// <returns></returns>
        public Func<object, object, bool> Adapt(MethodInfo method, Type inputType)
        {
#if NETCOREAPP2_0 || NET461
            var proxyMethod = ProxyMethodEmitter.CreateProxyMethod(method, inputType);
            return (listener, data) => proxyMethod(listener, data, _factory);
#elif NETSTANDARD2_0
            throw new PlatformNotSupportedException("This platform does not support creating proxy types and methods.");
#else
#error Target frameworks should be updated
#endif
        }
    }
}
