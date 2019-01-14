// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DiagnosticAdapter;

namespace System.Diagnostics
{
    /// <summary>
    /// 诊断监听扩展
    /// </summary>
    public static class DiagnosticListenerExtensions
    {
        /// <summary>
        /// 订阅适配器
        /// </summary>
        /// <param name="diagnostic">诊断监听</param>
        /// <param name="target">监听目标</param>
        /// <returns></returns>
        public static IDisposable SubscribeWithAdapter(this DiagnosticListener diagnostic, object target)
        {
            var adapter = new DiagnosticSourceAdapter(target);
            return diagnostic.Subscribe(adapter, (Predicate<string>)adapter.IsEnabled);
        }
        /// <summary>
        /// 订阅适配器
        /// </summary>
        /// <param name="diagnostic">诊断监听</param>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许</param>
        /// <returns></returns>
        public static IDisposable SubscribeWithAdapter(
            this DiagnosticListener diagnostic,
            object target,
            Func<string, bool> isEnabled)
        {
            var adapter = new DiagnosticSourceAdapter(target, isEnabled);
            return diagnostic.Subscribe(adapter, (Predicate<string>)adapter.IsEnabled);
        }
        /// <summary>
        /// 订阅适配器
        /// </summary>
        /// <param name="diagnostic">诊断监听</param>
        /// <param name="target">监听目标</param>
        /// <param name="isEnabled">是否允许</param>
        /// <returns></returns>
        public static IDisposable SubscribeWithAdapter(
            this DiagnosticListener diagnostic,
            object target,
            Func<string, object, object, bool> isEnabled)
        {
            var adapter = new DiagnosticSourceAdapter(target, isEnabled);
            return diagnostic.Subscribe(adapter, (Predicate<string>)adapter.IsEnabled);
        }
    }
}
