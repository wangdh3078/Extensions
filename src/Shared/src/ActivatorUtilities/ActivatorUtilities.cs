// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;

#if ActivatorUtilities_In_DependencyInjection
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection
#else
namespace Microsoft.Extensions.Internal
#endif
{
    /// <summary>
    /// Helper code for the various activator services.
    /// </summary>

#if ActivatorUtilities_In_DependencyInjection
    public
#else
    // Do not take a dependency on this class unless you are explicitly trying to avoid taking a
    // dependency on Microsoft.AspNetCore.DependencyInjection.Abstractions.
    internal
#endif
    static class ActivatorUtilities
    {
        private static readonly MethodInfo GetServiceInfo =
            GetMethodInfo<Func<IServiceProvider, Type, Type, bool, object>>((sp, t, r, c) => GetService(sp, t, r, c));

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="provider">用于解决依赖关系的服务提供程序</param>
        /// <param name="instanceType">实例类型</param>
        /// <param name="parameters"><paramref name ="provider"/>未提供的构造方法参数。</param>
        /// <returns></returns>
        public static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters)
        {
            int bestLength = -1;
            var seenPreferred = false;

            ConstructorMatcher bestMatcher = null;

            if (!instanceType.GetTypeInfo().IsAbstract)
            {
                foreach (var constructor in instanceType
                    .GetTypeInfo()
                    .DeclaredConstructors
                    .Where(c => !c.IsStatic && c.IsPublic))
                {
                    var matcher = new ConstructorMatcher(constructor);
                    var isPreferred = constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), false);
                    var length = matcher.Match(parameters);

                    if (isPreferred)
                    {
                        if (seenPreferred)
                        {
                            ThrowMultipleCtorsMarkedWithAttributeException();
                        }

                        if (length == -1)
                        {
                            ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
                        }
                    }

                    if (isPreferred || bestLength < length)
                    {
                        bestLength = length;
                        bestMatcher = matcher;
                    }

                    seenPreferred |= isPreferred;
                }
            }

            if (bestMatcher == null)
            {
                var message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.";
                throw new InvalidOperationException(message);
            }

            return bestMatcher.CreateInstance(provider);
        }

        /// <summary>
        /// 创建工厂委托
        /// </summary>
        /// <param name="instanceType">实例对象类型</param>
        /// <param name="argumentTypes">
        ///参数类型
        /// </param>
        /// <returns>
        ///将使用<see cref ="IServiceProvider"/>实例化instanceType的工厂以
        ///及包含与argumentTypes中定义的类型匹配的对象的参数数组
        /// </returns>
        public static ObjectFactory CreateFactory(Type instanceType, Type[] argumentTypes)
        {
            FindApplicableConstructor(instanceType, argumentTypes, out ConstructorInfo constructor, out int?[] parameterMap);

            var provider = Expression.Parameter(typeof(IServiceProvider), "provider");
            var argumentArray = Expression.Parameter(typeof(object[]), "argumentArray");
            var factoryExpressionBody = BuildFactoryExpression(constructor, parameterMap, provider, argumentArray);

            var factoryLamda = Expression.Lambda<Func<IServiceProvider, object[], object>>(
                factoryExpressionBody, provider, argumentArray);

            var result = factoryLamda.Compile();
            return result.Invoke;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="provider">用于解决依赖关系的服务提供程序</param>
        /// <param name="parameters">C<paramref name ="provider"/>未提供的构造方法参数</param>
        /// <returns></returns>
        public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters)
        {
            return (T)CreateInstance(provider, typeof(T), parameters);
        }


        /// <summary>
        /// 从服务提供者检索给定类型的实例。 如果找不到，则直接实例化。
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider">用于解决依赖关系的服务提供程序</param>
        /// <returns>已解析的服务或已创建的实例</returns>
        public static T GetServiceOrCreateInstance<T>(IServiceProvider provider)
        {
            return (T)GetServiceOrCreateInstance(provider, typeof(T));
        }

        /// <summary>
        /// 从服务提供者检索给定类型的实例。 如果找不到，则直接实例化。
        /// </summary>
        /// <param name="provider">用于解决依赖关系的服务提供程序</param>
        /// <param name="type">服务类型</param>
        /// <returns>已解析的服务或已创建的实例</returns>
        public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type)
        {
            return provider.GetService(type) ?? CreateInstance(provider, type);
        }
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        private static MethodInfo GetMethodInfo<T>(Expression<T> expr)
        {
            var mc = (MethodCallExpression)expr.Body;
            return mc.Method;
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="type"></param>
        /// <param name="requiredBy"></param>
        /// <param name="isDefaultParameterRequired"></param>
        /// <returns></returns>
        private static object GetService(IServiceProvider sp, Type type, Type requiredBy, bool isDefaultParameterRequired)
        {
            var service = sp.GetService(type);
            if (service == null && !isDefaultParameterRequired)
            {
                var message = $"Unable to resolve service for type '{type}' while attempting to activate '{requiredBy}'.";
                throw new InvalidOperationException(message);
            }
            return service;
        }
        /// <summary>
        /// 构建工厂表达式
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="parameterMap"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="factoryArgumentArray"></param>
        /// <returns></returns>
        private static Expression BuildFactoryExpression(
            ConstructorInfo constructor,
            int?[] parameterMap,
            Expression serviceProvider,
            Expression factoryArgumentArray)
        {
            var constructorParameters = constructor.GetParameters();
            var constructorArguments = new Expression[constructorParameters.Length];

            for (var i = 0; i < constructorParameters.Length; i++)
            {
                var constructorParameter = constructorParameters[i];
                var parameterType = constructorParameter.ParameterType;
                var hasDefaultValue = ParameterDefaultValue.TryGetDefaultValue(constructorParameter, out var defaultValue);

                if (parameterMap[i] != null)
                {
                    constructorArguments[i] = Expression.ArrayAccess(factoryArgumentArray, Expression.Constant(parameterMap[i]));
                }
                else
                {
                    var parameterTypeExpression = new Expression[] { serviceProvider,
                        Expression.Constant(parameterType, typeof(Type)),
                        Expression.Constant(constructor.DeclaringType, typeof(Type)),
                        Expression.Constant(hasDefaultValue) };
                    constructorArguments[i] = Expression.Call(GetServiceInfo, parameterTypeExpression);
                }

                // Support optional constructor arguments by passing in the default value
                // when the argument would otherwise be null.
                if (hasDefaultValue)
                {
                    var defaultValueExpression = Expression.Constant(defaultValue);
                    constructorArguments[i] = Expression.Coalesce(constructorArguments[i], defaultValueExpression);
                }

                constructorArguments[i] = Expression.Convert(constructorArguments[i], parameterType);
            }

            return Expression.New(constructor, constructorArguments);
        }
        /// <summary>
        /// 查找应用构造函数
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="argumentTypes"></param>
        /// <param name="matchingConstructor"></param>
        /// <param name="parameterMap"></param>
        private static void FindApplicableConstructor(
            Type instanceType,
            Type[] argumentTypes,
            out ConstructorInfo matchingConstructor,
            out int?[] parameterMap)
        {
            matchingConstructor = null;
            parameterMap = null;

            if (!TryFindPreferredConstructor(instanceType, argumentTypes, ref matchingConstructor, ref parameterMap) &&
                !TryFindMatchingConstructor(instanceType, argumentTypes, ref matchingConstructor, ref parameterMap))
            {
                var message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.";
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// 尝试根据提供的参数类型查找构造函数
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="argumentTypes"></param>
        /// <param name="matchingConstructor"></param>
        /// <param name="parameterMap"></param>
        /// <returns></returns>
        private static bool TryFindMatchingConstructor(
            Type instanceType,
            Type[] argumentTypes,
            ref ConstructorInfo matchingConstructor,
            ref int?[] parameterMap)
        {
            foreach (var constructor in instanceType.GetTypeInfo().DeclaredConstructors)
            {
                if (constructor.IsStatic || !constructor.IsPublic)
                {
                    continue;
                }

                if (TryCreateParameterMap(constructor.GetParameters(), argumentTypes, out int?[] tempParameterMap))
                {
                    if (matchingConstructor != null)
                    {
                        throw new InvalidOperationException($"Multiple constructors accepting all given argument types have been found in type '{instanceType}'. There should only be one applicable constructor.");
                    }

                    matchingConstructor = constructor;
                    parameterMap = tempParameterMap;
                }
            }

            return matchingConstructor != null;
        }

        /// <summary>
        /// 试图找到标有ActivatorUtilitiesConstructorAttribute的构造函数
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="argumentTypes"></param>
        /// <param name="matchingConstructor"></param>
        /// <param name="parameterMap"></param>
        /// <returns></returns>
        private static bool TryFindPreferredConstructor(
            Type instanceType,
            Type[] argumentTypes,
            ref ConstructorInfo matchingConstructor,
            ref int?[] parameterMap)
        {
            var seenPreferred = false;
            foreach (var constructor in instanceType.GetTypeInfo().DeclaredConstructors)
            {
                if (constructor.IsStatic || !constructor.IsPublic)
                {
                    continue;
                }

                if (constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), false))
                {
                    if (seenPreferred)
                    {
                        ThrowMultipleCtorsMarkedWithAttributeException();
                    }

                    if (!TryCreateParameterMap(constructor.GetParameters(), argumentTypes, out int?[] tempParameterMap))
                    {
                        ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
                    }

                    matchingConstructor = constructor;
                    parameterMap = tempParameterMap;
                    seenPreferred = true;
                }
            }

            return matchingConstructor != null;
        }

        // Creates an injective parameterMap from givenParameterTypes to assignable constructorParameters.
        // Returns true if each given parameter type is assignable to a unique; otherwise, false.
        private static bool TryCreateParameterMap(ParameterInfo[] constructorParameters, Type[] argumentTypes, out int?[] parameterMap)
        {
            parameterMap = new int?[constructorParameters.Length];

            for (var i = 0; i < argumentTypes.Length; i++)
            {
                var foundMatch = false;
                var givenParameter = argumentTypes[i].GetTypeInfo();

                for (var j = 0; j < constructorParameters.Length; j++)
                {
                    if (parameterMap[j] != null)
                    {
                        // This ctor parameter has already been matched
                        continue;
                    }

                    if (constructorParameters[j].ParameterType.GetTypeInfo().IsAssignableFrom(givenParameter))
                    {
                        foundMatch = true;
                        parameterMap[j] = i;
                        break;
                    }
                }

                if (!foundMatch)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 构造函数匹配器
        /// </summary>
        private class ConstructorMatcher
        {
            /// <summary>
            /// 构造函数信息
            /// </summary>
            private readonly ConstructorInfo _constructor;
            /// <summary>
            /// 参数集合
            /// </summary>
            private readonly ParameterInfo[] _parameters;
            /// <summary>
            /// 参数值集合
            /// </summary>
            private readonly object[] _parameterValues;

            private readonly bool[] _parameterValuesSet;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="constructor">构造函数信息</param>
            public ConstructorMatcher(ConstructorInfo constructor)
            {
                _constructor = constructor;
                _parameters = _constructor.GetParameters();
                _parameterValuesSet = new bool[_parameters.Length];
                _parameterValues = new object[_parameters.Length];
            }
            /// <summary>
            /// 匹配
            /// </summary>
            /// <param name="givenParameters">给定参数</param>
            /// <returns></returns>
            public int Match(object[] givenParameters)
            {
                var applyIndexStart = 0;
                var applyExactLength = 0;
                for (var givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
                {
                    var givenType = givenParameters[givenIndex]?.GetType().GetTypeInfo();
                    var givenMatched = false;

                    for (var applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
                    {
                        if (_parameterValuesSet[applyIndex] == false &&
                            _parameters[applyIndex].ParameterType.GetTypeInfo().IsAssignableFrom(givenType))
                        {
                            givenMatched = true;
                            _parameterValuesSet[applyIndex] = true;
                            _parameterValues[applyIndex] = givenParameters[givenIndex];
                            if (applyIndexStart == applyIndex)
                            {
                                applyIndexStart++;
                                if (applyIndex == givenIndex)
                                {
                                    applyExactLength = applyIndex;
                                }
                            }
                        }
                    }

                    if (givenMatched == false)
                    {
                        return -1;
                    }
                }
                return applyExactLength;
            }
            /// <summary>
            /// 创建实例
            /// </summary>
            /// <param name="provider"></param>
            /// <returns></returns>
            public object CreateInstance(IServiceProvider provider)
            {
                for (var index = 0; index != _parameters.Length; index++)
                {
                    if (_parameterValuesSet[index] == false)
                    {
                        var value = provider.GetService(_parameters[index].ParameterType);
                        if (value == null)
                        {
                            if (!ParameterDefaultValue.TryGetDefaultValue(_parameters[index], out var defaultValue))
                            {
                                throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                            }
                            else
                            {
                                _parameterValues[index] = defaultValue;
                            }
                        }
                        else
                        {
                            _parameterValues[index] = value;
                        }
                    }
                }

                try
                {
                    return _constructor.Invoke(_parameterValues);
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    // The above line will always throw, but the compiler requires we throw explicitly.
                    throw;
                }
            }
        }

        private static void ThrowMultipleCtorsMarkedWithAttributeException()
        {
            throw new InvalidOperationException($"Multiple constructors were marked with {nameof(ActivatorUtilitiesConstructorAttribute)}.");
        }

        private static void ThrowMarkedCtorDoesNotTakeAllProvidedArguments()
        {
            throw new InvalidOperationException($"Constructor marked with {nameof(ActivatorUtilitiesConstructorAttribute)} does not accept all given argument types.");
        }
    }
}
