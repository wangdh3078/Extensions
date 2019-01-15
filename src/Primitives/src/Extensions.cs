// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// StringBuilder扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 将给定的<see cref ="StringSegment"/>添加到<see cref ="StringBuilder"/>。
        /// </summary>
        /// <param name="builder">要添加的<see cref ="StringBuilder"/>。</param>
        /// <param name="segment">要添加的<see cref ="StringSegment"/>。</param>
        /// <returns></returns>
        public static StringBuilder Append(this StringBuilder builder, StringSegment segment)
        {
            return builder.Append(segment.Buffer, segment.Offset, segment.Length);
        }
    }
}
