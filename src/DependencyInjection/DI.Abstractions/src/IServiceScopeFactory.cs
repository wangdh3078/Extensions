// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 用于创建<see cref ="IServiceScope"/>实例的工厂，用于在范围内创建服务。
    /// </summary>
    public interface IServiceScopeFactory
    {
        /// <summary>
        /// 创建一个<see cref ="IServiceScope"/>，
        /// 其中包含一个<see cref ="IServiceProvider"/>，用于解析新创建的范围的依赖关系。
        /// </summary>
        /// <returns>
        /// <see cref ="IServiceScope"/>控制范围的生命周期。
        /// 处理完毕后，还将处理从<see cref ="IServiceScope.ServiceProvider"/>解析的任何作用域服务。
        /// </returns>
        IServiceScope CreateScope();
    }
}
