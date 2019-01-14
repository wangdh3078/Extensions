using Microsoft.Extensions.DependencyInjection;
using System;

namespace sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<IUser, User>()
                .AddTransient(typeof(IRole), typeof(Role));
            var provider = services.BuildServiceProvider();
            var user = provider.GetService<IUser>();
            Console.ReadKey();
        }
    }
}
