// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 服务提供程序引擎范围
    /// </summary>
    internal class ServiceProviderEngineScope : IServiceScope, IServiceProvider
    {
        // 仅用于测试
        internal Action<object> _captureDisposableCallback;
        /// <summary>
        /// 
        /// </summary>
        private List<IDisposable> _disposables;
        /// <summary>
        /// 已经回收
        /// </summary>
        private bool _disposed;
        /// <summary>
        /// 服务提供程序引擎范围-构造函数
        /// </summary>
        /// <param name="engine">服务提供程序引擎</param>
        public ServiceProviderEngineScope(ServiceProviderEngine engine)
        {
            Engine = engine;
        }
        /// <summary>
        /// 实现服务字典
        /// </summary>
        internal Dictionary<ServiceCacheKey, object> ResolvedServices { get; } = new Dictionary<ServiceCacheKey, object>();
        /// <summary>
        /// 服务提供程序引擎
        /// </summary>
        public ServiceProviderEngine Engine { get; }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (_disposed)
            {
                ThrowHelper.ThrowObjectDisposedException();
            }

            return Engine.GetService(serviceType, this);
        }
        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider => this;
        /// <summary>
        /// 回收释放资源
        /// </summary>
        public void Dispose()
        {
            lock (ResolvedServices)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                if (_disposables != null)
                {
                    for (var i = _disposables.Count - 1; i >= 0; i--)
                    {
                        var disposable = _disposables[i];
                        disposable.Dispose();
                    }

                    _disposables.Clear();
                }

                ResolvedServices.Clear();
            }
        }

        internal object CaptureDisposable(object service)
        {
            _captureDisposableCallback?.Invoke(service);

            if (!ReferenceEquals(this, service))
            {
                if (service is IDisposable disposable)
                {
                    lock (ResolvedServices)
                    {
                        if (_disposables == null)
                        {
                            _disposables = new List<IDisposable>();
                        }

                        _disposables.Add(disposable);
                    }
                }
            }
            return service;
        }
    }
}
