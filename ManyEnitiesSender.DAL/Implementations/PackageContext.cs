using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ManyEntitiesSender.DAL.Implementations
{
    public class PackageContext : DbContext, IPackageContext
    {
        public PackageContext(DbContextOptions options) : base(options)
        {
            // Вариант 2: Раскомментировать строку для создания структуры базы
            this.Database.EnsureCreated();
        }

        public PackageContext Context => this;

        public DbSet<Body> Body { get; set; }
        public DbSet<Hand> Hands { get; set; }
        public DbSet<Leg> Legs { get; set; }
        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class, IEntity
        {
            return this.Set<TEntity>();
        }
    }
}
