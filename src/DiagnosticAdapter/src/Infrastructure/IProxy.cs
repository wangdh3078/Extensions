// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DiagnosticAdapter.Infrastructure
{
    /// <summary>
    /// 代理
    /// </summary>
    public interface IProxy
    {
        /// <summary>
        ///展开底层对象并执行转换为<typeparamref name ="T"/>。
        /// </summary>
        /// <typeparam name="T">底层对象的类型。</typeparam>
        /// <returns>基础对象。</returns>
        T Upwrap<T>();
    }
}
