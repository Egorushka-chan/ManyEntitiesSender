using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.BLL.Services.Implementations;
using ManyEntitiesSender.DAL.Implementations;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace ManyEntitiesSender.BLL
{
    public static class InjectorBLL
    {
        public static void InjectBLL(this IServiceCollection services)
        {
            services.AddScoped<IPackageGetter, PackageGetter>();
            services.AddTransient<IObjectGenerator, RandomObjectGenerator>();
            services.AddScoped<IRedisProvider, RedisProvider>();
        }
    }
}
