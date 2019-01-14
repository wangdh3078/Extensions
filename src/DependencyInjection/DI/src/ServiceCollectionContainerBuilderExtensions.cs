// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务集合容器构造扩展
    /// </summary>
    public static class ServiceCollectionContainerBuilderExtensions
    {
        /// <summary>
        ///构建服务提供程序
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>

        public static ServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            return BuildServiceProvider(services, ServiceProviderOptions.Default);
        }

        /// <summary>
        /// 构建服务提供程序
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="validateScopes">是否验证服务范围</param>
        /// <returns></returns>
        public static ServiceProvider BuildServiceProvider(this IServiceCollection services, bool validateScopes)
        {
            return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = validateScopes });
        }

        /// <summary>
        /// 构建服务提供程序
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="options">服务提供程序选项</param>
        /// <returns></returns>
        public static ServiceProvider BuildServiceProvider(this IServiceCollection services, ServiceProviderOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return new ServiceProvider(services, options);
        }
    }
}
