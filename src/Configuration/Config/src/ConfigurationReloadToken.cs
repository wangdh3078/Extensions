// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 实现 <see cref="IChangeToken"/>
    /// </summary>
    public class ConfigurationReloadToken : IChangeToken
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 指示此令牌是否将主动引发回调。 最终仍然可以保证调用回调。
        /// </summary>
        public bool ActiveChangeCallbacks => true;

        /// <summary>
        /// 获取一个值，该值指示是否发生了更改。
        /// </summary>
        public bool HasChanged => _cts.IsCancellationRequested;

        /// <summary>
        /// 注册将在条目更改时调用的回调。 
        /// <see cref ="IChangeToken.HasChanged"/>必须在调用回调之前设置。
        /// </summary>
        /// <param name="callback">要调用的回调。</param>
        /// <param name="state">要传递回调的状态。</param>
        /// <returns></returns>
        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => _cts.Token.Register(callback, state);

        /// <summary>
        /// 用于在重新加载时触发更改令牌。
        /// </summary>
        public void OnReload() => _cts.Cancel();
    }
}
