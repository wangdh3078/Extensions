// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    internal struct ResultCache
    {
        /// <summary>
        /// 默认ResultCache  
        /// </summary>
        public static ResultCache None { get; } = new ResultCache(CallSiteResultCacheLocation.None, ServiceCacheKey.Empty);

        internal ResultCache(CallSiteResultCacheLocation lifetime, ServiceCacheKey cacheKey)
        {
            Location = lifetime;
            Key = cacheKey;
        }

        public ResultCache(ServiceLifetime lifetime, Type type, int slot)
        {
            Debug.Assert(lifetime == ServiceLifetime.Transient || type != null);

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    Location = CallSiteResultCacheLocation.Root;
                    break;
                case ServiceLifetime.Scoped:
                    Location = CallSiteResultCacheLocation.Scope;
                    break;
                case ServiceLifetime.Transient:
                    Location = CallSiteResultCacheLocation.Dispose;
                    break;
                default:
                    Location = CallSiteResultCacheLocation.None;
                    break;
            }
            Key = new ServiceCacheKey(type, slot);
        }
        /// <summary>
        /// 当前服务实例缓存位置
        /// </summary>
        public CallSiteResultCacheLocation Location { get; set; }
        /// <summary>
        /// 当前服务实例所缓存的使用Key
        /// </summary>
        public ServiceCacheKey Key { get; set; }
    }
}
