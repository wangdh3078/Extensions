// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.DiagnosticAdapter.Infrastructure;

namespace Microsoft.Extensions.DiagnosticAdapter.Internal
{
    /// <summary>
    /// 用于运行时创建代理对象的工厂
    /// </summary>
    public class ProxyFactory : IProxyFactory
    {
        /// <summary>
        /// 代理类型缓存
        /// </summary>
        private readonly ProxyTypeCache _cache = new ProxyTypeCache();
        /// <summary>
        /// 创建代理
        /// </summary>
        /// <typeparam name="TProxy">要创建的代理的类型。</typeparam>
        /// <param name="obj">要包装在代理中的对象。</param>
        /// <returns></returns>
        public TProxy CreateProxy<TProxy>(object obj)
        {
            if (obj == null)
            {
                return default(TProxy);
            }
            else if (typeof(TProxy).GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo()))
            {
                return (TProxy)obj;
            }

#if NETCOREAPP2_0 || NET461
            var type = ProxyTypeEmitter.GetProxyType(_cache, typeof(TProxy), obj.GetType());
            return (TProxy)Activator.CreateInstance(type, obj);
#elif NETSTANDARD2_0
            throw new PlatformNotSupportedException("This platform does not support creating proxy types and methods.");
#else
#error Target frameworks should be updated
#endif
        }
    }
}
