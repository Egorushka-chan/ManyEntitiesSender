using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ManyEntitiesSender.BLL.Services.Implementations
{
    public class PackageGetter(IPackageContext context, IRedisProvider redis, IOptions<PackageSettings> packageOptions) : IPackageGetter
    {
        /// <inheritdoc/>
        public async IAsyncEnumerable<List<TEntity>> GetPackageAsync<TEntity>(EntityFilterOptions filterOptions) where TEntity : class, IEntity, new()
        {
            Type type = typeof(TEntity);

            int totalCount = 0;
            if(filterOptions.PropertyFilter != null)
            {
                if(type == typeof(Body))
                {
                    totalCount = await context.DbSet<Body>()
                        .Where(el => el.Mightiness == filterOptions.PropertyFilter)
                        .CountAsync();
                }
                else if(type == typeof(Hand))
                {
                    totalCount = await context.DbSet<Hand>()
                        .Where(el => el.State == filterOptions.PropertyFilter)
                        .CountAsync();
                }
                else if(type == typeof(Leg))
                {
                    totalCount = await context.DbSet<Leg>()
                        .Where(el => el.State == filterOptions.PropertyFilter)
                        .CountAsync();
                }
                else
                    throw new ArgumentException($"{nameof(filterOptions)}.{nameof(filterOptions.PropertyFilter)}" +
                        $"cannot be not null, because filtering for required type {type.Name} not implemented");
            }
            else
                totalCount = await context.DbSet<TEntity>()
                    .CountAsync();

            int initial = filterOptions.Skip ?? 0;
            int totalTake = filterOptions.Take ?? totalCount;
            if(initial < 0)
                throw new ArgumentException("filterOptions.Skip must be >= 0", nameof(filterOptions));
            if(totalTake < 0)
                throw new ArgumentException("filterOptions.Take must be >= 0", nameof(filterOptions));

            int packageCount = packageOptions.Value.PackageCount;
            int iterationCounts = totalTake / packageCount;
            if (totalTake % iterationCounts != 0)
                iterationCounts++;
                
            for(int iteration = 0; iteration < iterationCounts; iteration++)
            {
                int skip = initial + (iteration * packageCount);
                int take = initial + ((iteration + 1) * packageCount);

                List<TEntity> entities = new List<TEntity>();
                if(filterOptions.PropertyFilter != null)
                {
                    if(type == typeof(Body))
                    {
                        entities.AddRange(await context.DbSet<Body>()
                        .Where(el => el.Mightiness == filterOptions.PropertyFilter)
                        .Skip(skip)
                        .Take(take)
                        .Cast<TEntity>()
                        .ToListAsync());
                    }
                    else if(type == typeof(Hand))
                    {
                        entities.AddRange(await context.DbSet<Hand>()
                        .Where(el => el.State == filterOptions.PropertyFilter)
                        .Skip(skip)
                        .Take(take)
                        .Cast<TEntity>()
                        .ToListAsync());
                    }
                    else if(type == typeof(Leg))
                    {
                        entities.AddRange(await context.DbSet<Leg>()
                        .Where(el => el.State == filterOptions.PropertyFilter)
                        .Skip(skip)
                        .Take(take)
                        .Cast<TEntity>()
                        .ToListAsync());
                    }
                    else
                        throw new ArgumentException($"{nameof(filterOptions)}.{nameof(filterOptions.PropertyFilter)}" +
                            $"cannot be not null, because filtering for required type {type.Name} not implemented");
                } 
                else
                {
                    entities.AddRange(await context.DbSet<TEntity>()
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync());
                }
                yield return entities;
            }
            
        }
    }
}
