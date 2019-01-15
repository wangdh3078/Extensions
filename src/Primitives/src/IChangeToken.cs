// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// 传播已发生更改的通知。
    /// </summary>
    public interface IChangeToken
    {
        /// <summary>
        /// 获取一个值，该值指示是否发生了更改。
        /// </summary>
        bool HasChanged { get; }

        /// <summary>
        /// 指示此令牌是否会主动引发回调。 如果<c> false </c>，
        /// 令牌使用者必须轮询<see cref ="HasChanged"/>以检测更改。
        /// </summary>
        bool ActiveChangeCallbacks { get; }

        /// <summary>
        /// 注册将在条目更改时调用的回调。
        /// <see cref ="HasChanged"/>必须在调用回调之前设置。
        /// </summary>
        /// <param name="callback">要调用<see cref ="Action {Object}"/>。</param>
        /// <param name="state">要传递回调的状态。</param>
        /// <returns>一个<see cref ="IDisposable"/>，用于取消注册回调。</returns>
        IDisposable RegisterChangeCallback(Action<object> callback, object state);
    }
}
