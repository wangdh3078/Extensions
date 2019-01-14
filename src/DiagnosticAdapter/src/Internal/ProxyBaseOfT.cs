// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DiagnosticAdapter.Internal
{
    /// <summary>
    /// 泛型代理基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxyBase<T> : ProxyBase where T : class
    {
        //由反射使用，不要重命名。
        public readonly T Instance;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instance">实例</param>
        public ProxyBase(T instance)
            : base(typeof(T))
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            Instance = instance;
        }
        /// <summary>
        /// 基础实例
        /// </summary>
        public T UnderlyingInstance
        {
            get
            {
                return Instance;
            }
        }
        /// <summary>
        /// 作为对象的基础实例
        /// </summary>
        public override object UnderlyingInstanceAsObject
        {
            get
            {
                return Instance;
            }
        }
    }
}

