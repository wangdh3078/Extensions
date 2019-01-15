// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 删除回调注册
    /// </summary>
    public class PostEvictionCallbackRegistration
    {
        /// <summary>
        /// 删除回调
        /// </summary>
        public PostEvictionDelegate EvictionCallback { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object State { get; set; }
    }
}
