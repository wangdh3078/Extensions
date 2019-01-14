// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DiagnosticAdapter.Internal;

namespace Microsoft.Extensions.DiagnosticAdapter
{
    /// <summary>
    /// 诊断源适配器
    /// </summary>
    public class DiagnosticSourceAdapter : IObserver<KeyValuePair<string, object>>
    {
        /// <summary>
        /// 监听
        /// </summary>
        private readonly Listener _listener;
        /// <summary>
        /// 诊断源方法适配器
        /// </summary>
        private readonly IDiagnosticSourceMethodAdapter _methodAdapter;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">监听目标</param>
        public DiagnosticSourceAdapter(object target)
            : this(target, (Func<string, bool>)null, new ProxyDiagnosticSourceMethodAdapter())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许监听</param>
        public DiagnosticSourceAdapter(object target, Func<string, bool> isEnabled)
            : this(target, isEnabled, methodAdapter: new ProxyDiagnosticSourceMethodAdapter())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许监听</param>
        public DiagnosticSourceAdapter(object target, Func<string, object, object, bool> isEnabled)
            : this(target, isEnabled, methodAdapter: new ProxyDiagnosticSourceMethodAdapter())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许监听</param>
        /// <param name="methodAdapter">方法适配器</param>
        public DiagnosticSourceAdapter(
            object target,
            Func<string, bool> isEnabled,
            IDiagnosticSourceMethodAdapter methodAdapter)
            : this(target, isEnabled: (isEnabled == null) ? (Func<string, object, object, bool>)null : (a, b, c) => isEnabled(a), methodAdapter: methodAdapter)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许监听</param>
        /// <param name="methodAdapter">方法适配器</param>
        public DiagnosticSourceAdapter(
            object target,
            Func<string, object, object, bool> isEnabled,
            IDiagnosticSourceMethodAdapter methodAdapter)
        {
            _methodAdapter = methodAdapter;
            _listener = EnlistTarget(target, isEnabled);
        }

        private static Listener EnlistTarget(object target, Func<string, object, object, bool> isEnabled)
        {
            var listener = new Listener(target, isEnabled);

            var typeInfo = target.GetType().GetTypeInfo();
            var methodInfos = typeInfo.DeclaredMethods;
            foreach (var methodInfo in methodInfos)
            {
                var diagnosticNameAttribute = methodInfo.GetCustomAttribute<DiagnosticNameAttribute>();
                if (diagnosticNameAttribute != null)
                {
                    var subscription = new Subscription(methodInfo);
                    listener.Subscriptions.Add(diagnosticNameAttribute.Name, subscription);
                }
            }

            return listener;
        }
        /// <summary>
        /// 是否允许
        /// </summary>
        /// <param name="diagnosticName">诊断名称</param>
        /// <returns></returns>
        public bool IsEnabled(string diagnosticName)
        {
            return IsEnabled(diagnosticName, null);
        }
        /// <summary>
        /// 是否允许
        /// </summary>
        /// <param name="diagnosticName">诊断名称</param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public bool IsEnabled(string diagnosticName, object arg1, object arg2 = null)
        {
            if (_listener.Subscriptions.Count == 0)
            {
                return false;
            }

            return
                _listener.Subscriptions.ContainsKey(diagnosticName) &&
                (_listener.IsEnabled == null || _listener.IsEnabled(diagnosticName, arg1, arg2));
        }

        public void Write(string diagnosticName, object parameters)
        {
            if (parameters == null)
            {
                return;
            }

            Subscription subscription;
            if (!_listener.Subscriptions.TryGetValue(diagnosticName, out subscription))
            {
                return;
            }

            var succeeded = false;
            foreach (var adapter in subscription.Adapters)
            {
                if (adapter(_listener.Target, parameters))
                {
                    succeeded = true;
                    break;
                }
            }

            if (!succeeded)
            {
                var newAdapter = _methodAdapter.Adapt(subscription.MethodInfo, parameters.GetType());
                try
                {
                    succeeded = newAdapter(_listener.Target, parameters);
                }
                catch (InvalidProxyOperationException ex)
                {
                    throw new InvalidOperationException(
                        Resources.FormatConverter_UnableToGenerateProxy(subscription.MethodInfo.Name),
                        ex);
                }
                Debug.Assert(succeeded);

                subscription.Adapters.Add(newAdapter);
            }
        }

        void IObserver<KeyValuePair<string, object>>.OnNext(KeyValuePair<string, object> value)
        {
            Write(value.Key, value.Value);
        }

        void IObserver<KeyValuePair<string, object>>.OnError(Exception error)
        {
            // Do nothing
        }

        void IObserver<KeyValuePair<string, object>>.OnCompleted()
        {
            // Do nothing
        }
        /// <summary>
        /// 监听
        /// </summary>
        private class Listener
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="target">监听对象</param>
            /// <param name="isEnabled">是否允许监听</param>
            public Listener(object target, Func<string, object, object, bool> isEnabled)
            {
                Target = target;
                IsEnabled = isEnabled;

                Subscriptions = new Dictionary<string, Subscription>(StringComparer.Ordinal);
            }
            /// <summary>
            /// 是否允许监听
            /// </summary>
            public Func<string, object, object, bool> IsEnabled { get; }
            /// <summary>
            /// 监听对象
            /// </summary>
            public object Target { get; }
            /// <summary>
            /// 订阅
            /// </summary>
            public Dictionary<string, Subscription> Subscriptions { get; }
        }
        /// <summary>
        /// 订阅
        /// </summary>
        private class Subscription
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="methodInfo">方法信息</param>
            public Subscription(MethodInfo methodInfo)
            {
                MethodInfo = methodInfo;

                Adapters = new ConcurrentBag<Func<object, object, bool>>();
            }
            /// <summary>
            /// 适配器
            /// </summary>
            public ConcurrentBag<Func<object, object, bool>> Adapters { get; }
            /// <summary>
            /// 方法信息
            /// </summary>
            public MethodInfo MethodInfo { get; }
        }
    }
}
