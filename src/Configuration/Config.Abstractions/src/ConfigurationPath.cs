// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 用于操作配置路径的实用方法和常量
    /// </summary>
    public static class ConfigurationPath
    {
        /// <summary>
        ///分隔符“：”用于分隔路径中的各个键。
        /// </summary>
        public static readonly string KeyDelimiter = ":";

        /// <summary>
        /// 将路径段合并为一个路径。
        /// </summary>
        /// <param name="pathSegments">要合并的路径段。</param>
        /// <returns>组合路径。</returns>
        public static string Combine(params string[] pathSegments)
        {
            if (pathSegments == null)
            {
                throw new ArgumentNullException(nameof(pathSegments));
            }
            return string.Join(KeyDelimiter, pathSegments);
        }

        /// <summary>
        ///将路径段合并为一个路径。
        /// </summary>
        /// <param name="pathSegments">要合并的路径段。</param>
        /// <returns>组合路径</returns>
        public static string Combine(IEnumerable<string> pathSegments)
        {
            if (pathSegments == null)
            {
                throw new ArgumentNullException(nameof(pathSegments));
            }
            return string.Join(KeyDelimiter, pathSegments);
        }

        /// <summary>
        /// 从路径中提取最后一个路径段。
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>路径的最后一个路径段。</returns>
        public static string GetSectionKey(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? path : path.Substring(lastDelimiterIndex + 1);
        }

        /// <summary>
        /// 为给定路径提取与父节点对应的路径。
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>原始路径减去在其中找到的最后一个单独的段。 如果原始路径对应于顶级节点，则为空。</returns>
        public static string GetParentPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? null : path.Substring(0, lastDelimiterIndex);
        }
    }
}
