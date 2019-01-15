// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// 取消更改令牌
    /// </summary>
    public class CancellationChangeToken : IChangeToken
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        public CancellationChangeToken(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
        }

        /// <summary>
        /// 指示此令牌是否会主动引发回调。 如果<c> false </c>，
        /// 令牌使用者必须轮询<see cref ="HasChanged"/>以检测更改。
        /// </summary>
        public bool ActiveChangeCallbacks { get; private set; } = true;

        /// <summary>
        /// 获取一个值，该值指示是否发生了更改。
        /// </summary>
        public bool HasChanged => Token.IsCancellationRequested;
        /// <summary>
        /// 取消令牌
        /// </summary>
        private CancellationToken Token { get; }

        /// <summary>
        /// 注册将在条目更改时调用的回调。
        /// <see cref ="HasChanged"/>必须在调用回调之前设置。
        /// </summary>
        /// <param name="callback">要调用<see cref ="Action {Object}"/>。</param>
        /// <param name="state">要传递回调的状态。</param>
        /// <returns>一个<see cref ="IDisposable"/>，用于取消注册回调。</returns>
        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            // 不要将当前的ExecutionContext及其AsyncLocals捕获到令牌注册上，导致它们永远存在
            var restoreFlow = false;
            if (!ExecutionContext.IsFlowSuppressed())//指示当前是否已禁止执行上下文的流。
            {
                ExecutionContext.SuppressFlow();
                restoreFlow = true;
            }

            try
            {
                return Token.Register(callback, state);
            }
            catch (ObjectDisposedException)
            {
                // Reset the flag so that we can indicate to future callers that this wouldn't work.
                ActiveChangeCallbacks = false;
            }
            finally
            {
                // Restore the current ExecutionContext
                if (restoreFlow)
                {
                    ExecutionContext.RestoreFlow();
                }
            }

            return NullDisposable.Instance;
        }

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            public void Dispose()
            {
            }
        }
    }
}
