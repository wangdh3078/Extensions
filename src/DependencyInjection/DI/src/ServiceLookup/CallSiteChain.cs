// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 调用设置链
    /// </summary>
    internal class CallSiteChain
    {
        /// <summary>
        /// 调用设置链字典
        /// </summary>
        private readonly Dictionary<Type, ChainItemInfo> _callSiteChain;
        /// <summary>
        /// 调用设置链-构造函数
        /// </summary>
        public CallSiteChain()
        {
            _callSiteChain = new Dictionary<Type, ChainItemInfo>();
        }
        /// <summary>
        /// 检查循环依赖性
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public void CheckCircularDependency(Type serviceType)
        {
            if (_callSiteChain.ContainsKey(serviceType))
            {
                throw new InvalidOperationException(CreateCircularDependencyExceptionMessage(serviceType));
            }
        }
        /// <summary>
        /// 移除调用设置链
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public void Remove(Type serviceType)
        {
            _callSiteChain.Remove(serviceType);
        }
        /// <summary>
        /// 添加服务调用链
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        public void Add(Type serviceType, Type implementationType = null)
        {
            _callSiteChain[serviceType] = new ChainItemInfo(_callSiteChain.Count, implementationType);
        }
        /// <summary>
        /// 创建循环依赖项异常消息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private string CreateCircularDependencyExceptionMessage(Type type)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat(Resources.CircularDependencyException, TypeNameHelper.GetTypeDisplayName(type));
            messageBuilder.AppendLine();

            AppendResolutionPath(messageBuilder, type);

            return messageBuilder.ToString();
        }
        /// <summary>
        /// 附加解决路径
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="currentlyResolving"></param>
        private void AppendResolutionPath(StringBuilder builder, Type currentlyResolving = null)
        {
            foreach (var pair in _callSiteChain.OrderBy(p => p.Value.Order))
            {
                var serviceType = pair.Key;
                var implementationType = pair.Value.ImplementationType;
                if (implementationType == null || serviceType == implementationType)
                {
                    builder.Append(TypeNameHelper.GetTypeDisplayName(serviceType));
                }
                else
                {
                    builder.AppendFormat("{0}({1})",
                        TypeNameHelper.GetTypeDisplayName(serviceType),
                        TypeNameHelper.GetTypeDisplayName(implementationType));
                }

                builder.Append(" -> ");
            }

            builder.Append(TypeNameHelper.GetTypeDisplayName(currentlyResolving));
        }
        /// <summary>
        /// 调用链对象信息
        /// </summary>
        private struct ChainItemInfo
        {
            /// <summary>
            /// 排序
            /// </summary>
            public int Order { get; }
            /// <summary>
            /// 实现类型
            /// </summary>
            public Type ImplementationType { get; }

            public ChainItemInfo(int order, Type implementationType)
            {
                Order = order;
                ImplementationType = implementationType;
            }
        }
    }
}
