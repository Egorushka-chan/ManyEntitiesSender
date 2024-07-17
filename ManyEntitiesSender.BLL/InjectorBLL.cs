using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.BLL.Services.Implementations;
using ManyEntitiesSender.DAL;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace ManyEntitiesSender.BLL
{
    public static class InjectorBLL
    {
        public static void InjectBLL(this IServiceCollection services)
        {
            services.AddScoped<ICacheableGetter, CacheableGetter>();
            services.AddTransient<IObjectGenerator, RandomObjectGenerator>();
        }
    }
}
