// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#if ActivatorUtilities_In_DependencyInjection
namespace Microsoft.Extensions.DependencyInjection
#else
namespace Microsoft.Extensions.Internal
#endif
{

    /// <summary>
    ///对象工厂委托
    /// </summary>
    /// <param name="serviceProvider">服务驱动</param>
    /// <param name="arguments">其他构造函数参数。</param>
    /// <returns>实例化的类型。</returns>
#if ActivatorUtilities_In_DependencyInjection
    public
#else
    internal
#endif
    delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);
}
