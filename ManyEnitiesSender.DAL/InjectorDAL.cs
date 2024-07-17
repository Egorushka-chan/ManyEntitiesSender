using ManyEntitiesSender.DAL.Implementations;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ManyEntitiesSender.DAL
{
    public static class InjectorDAL
    {
        public static void InjectDAL(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IPackageContext, PackageContext>(opt => {
                opt.UseNpgsql(connectionString, sqlOpt=> sqlOpt.MigrationsAssembly("ManyEntitiesSender"));
            }, ServiceLifetime.Scoped);
        } 
    }
}
