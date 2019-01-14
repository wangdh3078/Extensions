// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 用于配置默认<see cref ="IServiceProvider"/>实现的各种行为的选项。
    /// </summary>
    public class ServiceProviderOptions
    {
        /// <summary>
        ///默认对象  避免在默认情况下分配对象
        /// </summary>
        internal static readonly ServiceProviderOptions Default = new ServiceProviderOptions();

        /// <summary>
        ///<c> true </c>执行检查验证范围服务永远不会从根提供程序解析; 否则<c>false</c>。
        ///默认为<c> false </c>。
        /// </summary>
        public bool ValidateScopes { get; set; }

        /// <summary>
        ///<c> true </c>执行检查，验证是否可以在<code> BuildServiceProvider </code>调用期间创建所有服务; 否则<c>false</c>。 默认为<c> false </c>。
        /// 注意：此检查不验证开放的泛型服务。
        /// </summary>
        public bool ValidateOnBuild { get; set; }
        /// <summary>
        /// 模式
        /// </summary>
        internal ServiceProviderMode Mode { get; set; } = ServiceProviderMode.Default;
    }
}
