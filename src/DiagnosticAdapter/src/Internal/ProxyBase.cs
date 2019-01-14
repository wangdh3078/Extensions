// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DiagnosticAdapter.Infrastructure;

namespace Microsoft.Extensions.DiagnosticAdapter.Internal
{
    /// <summary>
    /// 代理基类
    /// </summary>
    public abstract class ProxyBase : IProxy
    {
        /// <summary>
        /// 包裹的类型
        /// </summary>
        public readonly Type WrappedType;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wrappedType">包裹的类型</param>
        protected ProxyBase(Type wrappedType)
        {
            WrappedType = wrappedType;
        }

        // 由反射使用，不要重命名。
        public abstract object UnderlyingInstanceAsObject
        {
            get;
        }

        public T Upwrap<T>()
        {
            return (T)UnderlyingInstanceAsObject;
        }
    }
}

