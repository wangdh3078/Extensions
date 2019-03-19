using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// IOptions包装器返回选项实例。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsWrapper<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// 使用选项实例初始化包装器以返回。
        /// </summary>
        /// <param name="options">要返回的选项实例。</param>
        public OptionsWrapper(TOptions options)
        {
            Value = options;
        }

        /// <summary>
        /// 选项实例。
        /// </summary>
        public TOptions Value { get; }
    }
}
