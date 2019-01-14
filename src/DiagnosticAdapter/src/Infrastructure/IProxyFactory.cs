// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DiagnosticAdapter.Infrastructure
{
    /// <summary>
    /// 用于运行时创建代理对象的工厂。
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        ///创建一个可分配给<typeparamref name ="TProxy"/>的代理对象
        /// </summary>
        /// <typeparam name="TProxy">要创建的代理的类型。</typeparam>
        /// <param name="obj">要包装在代理中的对象。</param>
        /// <returns>代理对象，如果不需要代理，则为<paramref name ="obj"/>。</returns>
        TProxy CreateProxy<TProxy>(object obj);
    }
}
