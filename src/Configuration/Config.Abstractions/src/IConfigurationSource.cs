// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 表示应用程序的配置键/值的来源。
    /// </summary>
    public interface IConfigurationSource
    {
        /// <summary>
        /// 为此源构建<see cref ="IConfigurationProvider"/>。
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An <see cref="IConfigurationProvider"/></returns>
        IConfigurationProvider Build(IConfigurationBuilder builder);
    }
}
