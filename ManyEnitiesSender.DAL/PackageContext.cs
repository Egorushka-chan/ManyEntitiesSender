using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ManyEntitiesSender.DAL
{
    public class PackageContext : DbContext, IPackageContext
    {
        public PackageContext(DbContextOptions options) : base(options)
        {
            
        }

        public PackageContext Context => this;

        public DbSet<Package> Packages { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
    }
}
