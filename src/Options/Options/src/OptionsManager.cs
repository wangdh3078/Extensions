// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    ///IOptions和IOptionsSnapshot的实现。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly OptionsCache<TOptions> _cache = new OptionsCache<TOptions>(); // Note: this is a private cache

        /// <summary>
        ///使用指定的选项配置初始化新实例。
        /// </summary>
        /// <param name="factory">工厂用来创建选项。</param>
        public OptionsManager(IOptionsFactory<TOptions> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// 默认配置的TOptions实例，相当于Get(Options.DefaultName)。
        /// </summary>
        public TOptions Value
        {
            get
            {
                return Get(Options.DefaultName);
            }
        }

        /// <summary>
        /// 返回具有给定名称的已配置TOptions实例。
        /// </summary>
        public virtual TOptions Get(string name)
        {
            name = name ?? Options.DefaultName;

            // Store the options in our instance cache
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }
    }
}
