// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    internal class IEnumerableCallSite : ServiceCallSite
    {
        /// <summary>
        ///   当前注册的类型  (基类类型)
        /// </summary>
        internal Type ItemType { get; }
        /// <summary>
        ///  所有服务的ServiceCallSite数组
        /// </summary>
        internal ServiceCallSite[] ServiceCallSites { get; }

        public IEnumerableCallSite(ResultCache cache, Type itemType, ServiceCallSite[] serviceCallSites) : base(cache)
        {
            ItemType = itemType;
            ServiceCallSites = serviceCallSites;
        }

        public override Type ServiceType => typeof(IEnumerable<>).MakeGenericType(ItemType);
        public override Type ImplementationType  => ItemType.MakeArrayType();
        public override CallSiteKind Kind { get; } = CallSiteKind.IEnumerable;
    }
}
