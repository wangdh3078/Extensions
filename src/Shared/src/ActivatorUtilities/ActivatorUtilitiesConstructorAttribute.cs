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
    /// 使用<see cref ="ActivatorUtilities"/>标记激活类型时要使用的构造函数。
    /// </summary>

#if ActivatorUtilities_In_DependencyInjection
    public
#else
    // 除非您明确尝试避免依赖于Microsoft.AspNetCore.DependencyInjection.Abstractions，否则不要依赖此类。
    internal
#endif
    class ActivatorUtilitiesConstructorAttribute: Attribute
    {
    }
}
