// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 工厂服务调用设置
    /// </summary>
    internal class FactoryCallSite : ServiceCallSite
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public Func<IServiceProvider, object> Factory { get; }
        /// <summary>
        /// 工厂服务调用设置-构造函数
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="factory">服务工厂</param>

        public FactoryCallSite(ResultCache cache, Type serviceType, Func<IServiceProvider, object> factory) : base(cache)
        {
            Factory = factory;
            ServiceType = serviceType;
        }

        public override Type ServiceType { get; }
        public override Type ImplementationType => null;

        public override CallSiteKind Kind { get; } = CallSiteKind.Factory;
    }
}
