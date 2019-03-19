// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 由<see cref ="IOptionsMonitor {TOptions}"/>用于缓存TOptions实例。"
    /// </summary>
    /// <typeparam name="TOptions">要求的选项类型.</typeparam>
    public interface IOptionsMonitorCache<TOptions> where TOptions : class
    {
        /// <summary>
        /// 获取命名选项实例，或添加使用createOptions创建的新实例。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <param name="createOptions">用于创建新实例的func。</param>
        /// <returns>选项实例。</returns>
        TOptions GetOrAdd(string name, Func<TOptions> createOptions);

        /// <summary>
        /// 尝试向缓存添加新选项，如果名称已存在则返回false。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <param name="options">选项实例。</param>
        /// <returns>是否添加成功</returns>
        bool TryAdd(string name, TOptions options);

        /// <summary>
        /// 尝试删除选项实例。
        /// </summary>
        /// <param name="name">选项实例的名称。</param>
        /// <returns>是否删除成功</returns>
        bool TryRemove(string name);

        /// <summary>
        /// 清除缓存中的所有选项实例。
        /// </summary>
        void Clear();
    }
}
