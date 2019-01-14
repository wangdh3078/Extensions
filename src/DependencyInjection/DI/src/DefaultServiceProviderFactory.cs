using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///默认服务提供程序工厂
    /// </summary>
    public class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        /// <summary>
        /// 服务提供程序选项
        /// </summary>
        private readonly ServiceProviderOptions _options;

        /// <summary>
        /// 默认服务提供程序工厂-构造函数
        /// </summary>
        /// <seealso cref="ServiceProviderOptions.Default"/>
        public DefaultServiceProviderFactory() : this(ServiceProviderOptions.Default)
        {

        }

        /// <summary>
        /// 默认服务提供程序工厂-构造函数
        /// </summary>
        /// <param name="options">服务提供程序选项</param>
        public DefaultServiceProviderFactory(ServiceProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options;
        }

        /// <summary>
        /// 创建容器构建者
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            return services;
        }

       /// <summary>
       /// 创建服务提供程序
       /// </summary>
       /// <param name="containerBuilder">容器构建者</param>
       /// <returns></returns>
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            return containerBuilder.BuildServiceProvider(_options);
        }
    }
}
