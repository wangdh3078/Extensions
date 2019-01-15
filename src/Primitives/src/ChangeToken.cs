// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// 传播已发生更改的通知。
    /// </summary>
    public static class ChangeToken
    {
        /// <summary>
        /// 注册<paramref name ="changeTokenConsumer"/>动作，只要令牌产生变化就会被调用。
        /// </summary>
        /// <param name="changeTokenProducer">生成更改令牌。</param>
        /// <param name="changeTokenConsumer">令牌更改时调用的操作。</param>
        /// <returns></returns>
        public static IDisposable OnChange(Func<IChangeToken> changeTokenProducer, Action changeTokenConsumer)
        {
            if (changeTokenProducer == null)
            {
                throw new ArgumentNullException(nameof(changeTokenProducer));
            }
            if (changeTokenConsumer == null)
            {
                throw new ArgumentNullException(nameof(changeTokenConsumer));
            }

            return new ChangeTokenRegistration<Action>(changeTokenProducer, callback => callback(), changeTokenConsumer);
        }

        /// <summary>
        /// 注册<paramref name ="changeTokenConsumer"/>动作，只要令牌产生变化就会被调用。
        /// </summary>
        /// <param name="changeTokenProducer">生成更改令牌。</param>
        /// <param name="changeTokenConsumer">令牌更改时调用的操作。</param>
        /// <param name="state">消费者的状态。</param>
        /// <returns></returns>
        public static IDisposable OnChange<TState>(Func<IChangeToken> changeTokenProducer, Action<TState> changeTokenConsumer, TState state)
        {
            if (changeTokenProducer == null)
            {
                throw new ArgumentNullException(nameof(changeTokenProducer));
            }
            if (changeTokenConsumer == null)
            {
                throw new ArgumentNullException(nameof(changeTokenConsumer));
            }

            return new ChangeTokenRegistration<TState>(changeTokenProducer, changeTokenConsumer, state);
        }

        /// <summary>
        /// 更改令牌注册
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private class ChangeTokenRegistration<TState> : IDisposable
        {
            private readonly Func<IChangeToken> _changeTokenProducer;
            private readonly Action<TState> _changeTokenConsumer;
            private readonly TState _state;
            private IDisposable _disposable;

            private static readonly NoopDisposable _disposedSentinel = new NoopDisposable();

            public ChangeTokenRegistration(Func<IChangeToken> changeTokenProducer, Action<TState> changeTokenConsumer, TState state)
            {
                _changeTokenProducer = changeTokenProducer;
                _changeTokenConsumer = changeTokenConsumer;
                _state = state;

                var token = changeTokenProducer();

                RegisterChangeTokenCallback(token);
            }

            private void OnChangeTokenFired()
            {
                // The order here is important. We need to take the token and then apply our changes BEFORE
                // registering. This prevents us from possible having two change updates to process concurrently.
                //
                // If the token changes after we take the token, then we'll process the update immediately upon
                // registering the callback.
                var token = _changeTokenProducer();

                try
                {
                    _changeTokenConsumer(_state);
                }
                finally
                {
                    // We always want to ensure the callback is registered
                    RegisterChangeTokenCallback(token);
                }
            }

            private void RegisterChangeTokenCallback(IChangeToken token)
            {
                var registraton = token.RegisterChangeCallback(s => ((ChangeTokenRegistration<TState>)s).OnChangeTokenFired(), this);

                SetDisposable(registraton);
            }

            private void SetDisposable(IDisposable disposable)
            {
                // We don't want to transition from _disposedSentinel => anything since it's terminal
                // but we want to allow going from previously assigned disposable, to another
                // disposable.
                var current = Volatile.Read(ref _disposable);

                // If Dispose was called, then immediately dispose the disposable
                if (current == _disposedSentinel)
                {
                    disposable.Dispose();
                    return;
                }

                // Otherwise, try to update the disposable
                var previous = Interlocked.CompareExchange(ref _disposable, disposable, current);

                if (previous == _disposedSentinel)
                {
                    // The subscription was disposed so we dispose immediately and return
                    disposable.Dispose();
                }
                else if (previous == current)
                {
                    // We successfuly assigned the _disposable field to disposable
                }
                else
                {
                    // Sets can never overlap with other SetDisposable calls so we should never get into this situation
                    throw new InvalidOperationException("Somebody else set the _disposable field");
                }
            }

            public void Dispose()
            {
                // If the previous value is disposable then dispose it, otherwise,
                // now we've set the disposed sentinel
                Interlocked.Exchange(ref _disposable, _disposedSentinel).Dispose();
            }

            private class NoopDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}
