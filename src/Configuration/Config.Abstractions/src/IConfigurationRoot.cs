// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 表示<see cref ="IConfiguration"/>层次结构的根。
    /// </summary>
    public interface IConfigurationRoot : IConfiguration
    {
        /// <summary>
        /// 强制从底层<see cref ="IConfigurationProvider"/> s重新加载配置值。
        /// </summary>
        void Reload();

        /// <summary>
        /// 这个配置的<see cref ="IConfigurationProvider"/> s。
        /// </summary>
        IEnumerable<IConfigurationProvider> Providers { get; }
    }
}
