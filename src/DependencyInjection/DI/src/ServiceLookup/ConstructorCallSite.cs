// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 构造函数服务调用设置
    /// </summary>
    internal class ConstructorCallSite : ServiceCallSite
    {
        /// <summary>
        /// 实例化对象时所使用的构造器,当前构造器的最优构造器
        /// </summary>
        internal ConstructorInfo ConstructorInfo { get; }
        /// <summary>
        /// 当前构造器中所有参数的ServiceCallSite集合
        /// </summary>
        internal ServiceCallSite[] ParameterCallSites { get; }

        public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo) : this(cache, serviceType, constructorInfo, Array.Empty<ServiceCallSite>())
        {
        }

        public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo, ServiceCallSite[] parameterCallSites) : base(cache)
        {
            if (!serviceType.IsAssignableFrom(constructorInfo.DeclaringType))
            {
                throw new ArgumentException(Resources.FormatImplementationTypeCantBeConvertedToServiceType(constructorInfo.DeclaringType, serviceType));
            }

            ServiceType = serviceType;
            ConstructorInfo = constructorInfo;
            ParameterCallSites = parameterCallSites;
        }

        public override Type ServiceType { get; }

        public override Type ImplementationType => ConstructorInfo.DeclaringType;
        public override CallSiteKind Kind { get; } = CallSiteKind.Constructor;
    }
}
