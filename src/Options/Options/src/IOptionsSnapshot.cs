// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 用于在请求的生命周期内访问TOptions的值。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptionsSnapshot<out TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// 返回具有给定名称的已配置TOptions实例。
        /// </summary>
        TOptions Get(string name);
    }
}
