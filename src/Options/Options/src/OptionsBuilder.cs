// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    ///用于配置TOptions实例。
    /// </summary>
    /// <typeparam name="TOptions">要求的选项类型。</typeparam>
    public class OptionsBuilder<TOptions> where TOptions : class
    {
        /// <summary>
        /// TOptions实例的默认名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="services"><see cref ="IServiceCollection"/>用于正在配置的选项。</param>
        /// <param name="name">TOptions实例的默认名称，如果使用null Options.DefaultName。</param>
        public OptionsBuilder(IServiceCollection services, string name)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
            Name = name ?? Options.DefaultName;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        /// 注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure(Action<TOptions> configureOptions)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(Name, configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        /// 注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <typeparam name="TDep">操作使用的依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep>(Action<TOptions, TDep> configureOptions)
            where TDep : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        ///注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions)
            where TDep1 : class
            where TDep2 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        /// 注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IConfigureOptions<TOptions>>(
                sp => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        /// 注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <typeparam name="TDep4">操作使用的第四个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
            where TDep4 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IConfigureOptions<TOptions>>(
                sp => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    sp.GetRequiredService<TDep4>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        ///注册用于配置特定类型选项的操作。
        /// 注意：这些在所有<seealso cref ="PostConfigure(Action {TOptions})"/>之前运行。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <typeparam name="TDep4">操作使用的第四个依赖项。</typeparam>
        /// <typeparam name="TDep5">操作使用的第五个依赖项</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
            where TDep4 : class
            where TDep5 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IConfigureOptions<TOptions>>(
                sp => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    sp.GetRequiredService<TDep4>(),
                    sp.GetRequiredService<TDep5>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于配置特定类型选项的操作。
        /// 注意：这些都在<seealso cref ="Configure(Action {TOptions})"/>之后运行。
        /// </summary>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        public virtual OptionsBuilder<TOptions> PostConfigure(Action<TOptions> configureOptions)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddSingleton<IPostConfigureOptions<TOptions>>(new PostConfigureOptions<TOptions>(Name, configureOptions));
            return this;
        }

        /// <summary>
        ///注册用于发布配置特定类型选项的操作。
        /// 注意：这些都在<seealso cref ="Configure(Action {TOptions})"/>之后运行。
        /// </summary>
        /// <typeparam name="TDep">操作使用的依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep>(Action<TOptions, TDep> configureOptions)
            where TDep : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IPostConfigureOptions<TOptions>>(sp =>
                new PostConfigureOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions));
            return this;
        }

        /// <summary>
        /// 注册用于发布配置特定类型选项的操作。
        /// 注意：这些是在<seealso cref ="Configure(Action {TOptions})"/>之前运行的。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions)
            where TDep1 : class
            where TDep2 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IPostConfigureOptions<TOptions>>(sp =>
                new PostConfigureOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions));
            return this;
        }

        /// <summary>
        ///注册用于发布配置特定类型选项的操作。
        ///注意：这些是在<seealso cref ="Configure(Action {TOptions})"/>之前运行的。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IPostConfigureOptions<TOptions>>(
                sp => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        ///注册用于发布配置特定类型选项的操作。
        ///注意：这些是在<seealso cref ="Configure(Action {TOptions})"/>之前运行的。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <typeparam name="TDep4">动作使用的第四个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
            where TDep4 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IPostConfigureOptions<TOptions>>(
                sp => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    sp.GetRequiredService<TDep4>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        ///注册用于发布配置特定类型选项的操作。
        ///注意：这些是在<seealso cref ="Configure(Action {TOptions})"/>之前运行的。
        /// </summary>
        /// <typeparam name="TDep1">操作使用的第一个依赖项。</typeparam>
        /// <typeparam name="TDep2">操作使用的第二个依赖项。</typeparam>
        /// <typeparam name="TDep3">操作使用的第三个依赖项。</typeparam>
        /// <typeparam name="TDep4">动作使用的第四个依赖项。</typeparam>
        /// <typeparam name="TDep5">动作使用的第五个依赖项。</typeparam>
        /// <param name="configureOptions">用于配置选项的操作。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions)
            where TDep1 : class
            where TDep2 : class
            where TDep3 : class
            where TDep4 : class
            where TDep5 : class
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            Services.AddTransient<IPostConfigureOptions<TOptions>>(
                sp => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(
                    Name,
                    sp.GetRequiredService<TDep1>(),
                    sp.GetRequiredService<TDep2>(),
                    sp.GetRequiredService<TDep3>(),
                    sp.GetRequiredService<TDep4>(),
                    sp.GetRequiredService<TDep5>(),
                    configureOptions));
            return this;
        }

        /// <summary>
        ///使用默认失败消息为选项类型注册验证操作。
        /// </summary>
        /// <param name="validation">验证功能。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation)
            => Validate(validation: validation, failureMessage: "A validation error has occured.");

        /// <summary>
        /// 注册选项类型的验证操作。
        /// </summary>
        /// <param name="validation">验证功能。</param>
        /// <param name="failureMessage">验证失败时使用的失败消息。</param>
        /// <returns>The current OptionsBuilder.</returns>
        public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation, string failureMessage)
        {
            if (validation == null)
            {
                throw new ArgumentNullException(nameof(validation));
            }

            Services.AddSingleton<IValidateOptions<TOptions>>(new ValidateOptions<TOptions>(Name, validation, failureMessage));
            return this;
        }
    }
}
