// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace Microsoft.Extensions.Internal
{
    /// <summary>
    /// 类型名称帮助
    /// </summary>
    internal class TypeNameHelper
    {
        /// <summary>
        /// 默认嵌套类型分隔符
        /// </summary>
        private const char DefaultNestedTypeDelimiter = '+';
        /// <summary>
        /// 内置类型名称
        /// </summary>
        private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
        {
            { typeof(void), "void" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(object), "object" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(ushort), "ushort" }
        };
        /// <summary>
        /// 获取类型显示名称
        /// </summary>
        /// <param name="item">对象</param>
        /// <param name="fullName">是否显示完整名称</param>
        /// <returns></returns>
        public static string GetTypeDisplayName(object item, bool fullName = true)
        {
            return item == null ? null : GetTypeDisplayName(item.GetType(), fullName);
        }

        /// <summary>
        /// 显示类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="fullName"><c> true </c>打印完全限定名称。</param>
        /// <param name="includeGenericParameterNames"><c> true </c>包含通用参数名称。</param>
        /// <param name="includeGenericParameters"><c> true </c>包含通用参数。</param>
        /// <param name="nestedTypeDelimiter">用作嵌套类型名称中的分隔符的字符</param>
        /// <returns></returns>
        public static string GetTypeDisplayName(Type type, bool fullName = true, bool includeGenericParameterNames = false, bool includeGenericParameters = true, char nestedTypeDelimiter = DefaultNestedTypeDelimiter)
        {
            var builder = new StringBuilder();
            ProcessType(builder, type, new DisplayNameOptions(fullName, includeGenericParameterNames, includeGenericParameters, nestedTypeDelimiter));
            return builder.ToString();
        }
        /// <summary>
        /// 显示类型名称
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        /// <param name="type">类型</param>
        /// <param name="options">显示名称选项</param>
        private static void ProcessType(StringBuilder builder, Type type, in DisplayNameOptions options)
        {
            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                ProcessGenericType(builder, type, genericArguments, genericArguments.Length, options);
            }
            else if (type.IsArray)
            {
                ProcessArrayType(builder, type, options);
            }
            else if (_builtInTypeNames.TryGetValue(type, out var builtInName))
            {
                builder.Append(builtInName);
            }
            else if (type.IsGenericParameter)
            {
                if (options.IncludeGenericParameterNames)
                {
                    builder.Append(type.Name);
                }
            }
            else
            {
                var name = options.FullName ? type.FullName : type.Name;
                builder.Append(name);

                if (options.NestedTypeDelimiter != DefaultNestedTypeDelimiter)
                {
                    builder.Replace(DefaultNestedTypeDelimiter, options.NestedTypeDelimiter, builder.Length - name.Length, name.Length);
                }
            }
        }
        /// <summary>
        /// 显示数组类型
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        /// <param name="type">类型</param>
        /// <param name="options">显示名称选项</param>
        private static void ProcessArrayType(StringBuilder builder, Type type, in DisplayNameOptions options)
        {
            var innerType = type;
            while (innerType.IsArray)
            {
                innerType = innerType.GetElementType();
            }

            ProcessType(builder, innerType, options);

            while (type.IsArray)
            {
                builder.Append('[');
                builder.Append(',', type.GetArrayRank() - 1);
                builder.Append(']');
                type = type.GetElementType();
            }
        }
        /// <summary>
        /// 显示泛型名称
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        /// <param name="type">类型</param>
        /// <param name="genericArguments">泛型参数</param>
        /// <param name="length">长度</param>
        /// <param name="options">显示名称选项</param>
        private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, in DisplayNameOptions options)
        {
            var offset = 0;
            if (type.IsNested)
            {
                offset = type.DeclaringType.GetGenericArguments().Length;
            }

            if (options.FullName)
            {
                if (type.IsNested)
                {
                    ProcessGenericType(builder, type.DeclaringType, genericArguments, offset, options);
                    builder.Append(options.NestedTypeDelimiter);
                }
                else if (!string.IsNullOrEmpty(type.Namespace))
                {
                    builder.Append(type.Namespace);
                    builder.Append('.');
                }
            }

            var genericPartIndex = type.Name.IndexOf('`');
            if (genericPartIndex <= 0)
            {
                builder.Append(type.Name);
                return;
            }

            builder.Append(type.Name, 0, genericPartIndex);

            if (options.IncludeGenericParameters)
            {
                builder.Append('<');
                for (var i = offset; i < length; i++)
                {
                    ProcessType(builder, genericArguments[i], options);
                    if (i + 1 == length)
                    {
                        continue;
                    }

                    builder.Append(',');
                    if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
                    {
                        builder.Append(' ');
                    }
                }
                builder.Append('>');
            }
        }
        /// <summary>
        /// 显示名称选项
        /// </summary>
        private readonly struct DisplayNameOptions
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="fullName">完全限定名称</param>
            /// <param name="includeGenericParameterNames">包含通用参数名称</param>
            /// <param name="includeGenericParameters">包含通用参数</param>
            /// <param name="nestedTypeDelimiter">用作嵌套类型名称中的分隔符的字符</param>
            public DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
            {
                FullName = fullName;
                IncludeGenericParameters = includeGenericParameters;
                IncludeGenericParameterNames = includeGenericParameterNames;
                NestedTypeDelimiter = nestedTypeDelimiter;
            }
            /// <summary>
            /// 完全限定名称
            /// </summary>
            public bool FullName { get; }
            /// <summary>
            /// 包含通用参数
            /// </summary>
            public bool IncludeGenericParameters { get; }
            /// <summary>
            /// 包含通用参数名称
            /// </summary>
            public bool IncludeGenericParameterNames { get; }
            /// <summary>
            /// 用作嵌套类型名称中的分隔符的字符
            /// </summary>
            public char NestedTypeDelimiter { get; }
        }
    }
}
