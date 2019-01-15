// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// 高速缓存条目到期时调用的回调签名。
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="reason">删除原因</param>
    /// <param name="state">注册回调时传递的信息。</param>
    public delegate void PostEvictionDelegate(object key, object value, EvictionReason reason, object state);
}
