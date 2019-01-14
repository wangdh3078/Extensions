// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 服务调用设置
    /// </summary>
    internal abstract class ServiceCallSite
    {
        /// <summary>
        ///服务调用设置-构造函数
        /// </summary>
        /// <param name="cache">服务实例对象的缓存配置</param>
        protected ServiceCallSite(ResultCache cache)
        {
            Cache = cache;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public abstract Type ServiceType { get; }
        /// <summary>
        /// 服务实现类型
        /// </summary>
        public abstract Type ImplementationType { get; }
        /// <summary>
        /// 当前CallSite所属的类型
        /// </summary>
        public abstract CallSiteKind Kind { get; }
        /// <summary>
        /// 服务实例对象的缓存配置
        /// </summary>
        public ResultCache Cache { get; }
    }
}
