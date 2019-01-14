// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务范围
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// <see cref ="IServiceProvider"/>用于解析范围的依赖关系。
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
