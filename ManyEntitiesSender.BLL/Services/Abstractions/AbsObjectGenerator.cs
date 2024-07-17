using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ManyEntitiesSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManyEntitiesSender.BLL.Services.Abstractions
{
    public abstract class AbsObjectGenerator(IPackageContext context, IOptions<PackageSettings> options, ILogger<AbsObjectGenerator> logger) : IObjectGenerator
    {
        public async Task EnsurePartsCount()
        {
            logger.LogDebug("Begin ensuring sets of entities");
            logger.LogTrace("Begin ensuring bodies");
            await Ensure<Body>();
            logger.LogTrace("Begin ensuring hands");
            await Ensure<Hand>();
            logger.LogTrace("Begin ensuring legs");
            await Ensure<Leg>();
            logger.LogDebug("Ensuring complete");
        }

        private async Task Ensure<TEntity>() where TEntity : class, IEntity, new()
        {
            TEntity checkEntity = new();
            if(!(checkEntity is Body || checkEntity is Hand || checkEntity is Leg))
                throw new NotImplementedException("Such entity type can't be created");

            int quantity = options.Value.PackageTotal;
            int count = await context.DbSet<TEntity>().CountAsync();
            int required = quantity - count;
            int package = options.Value.PackageCount;
            int iterations = required/package;

            if (required > 0) {
                foreach (int iteration in Enumerable.Range(1, iterations + 1)) {
                    List<TEntity> entities = new();

                    foreach (int i in Enumerable.Range(0, package)) {
                        int testNo = i * iteration;
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                        if(checkEntity is Body)
                        {
                            entities.Add(CreateBody(testNo) as TEntity);
                        }
                        else if(checkEntity is Hand)
                        {
                            entities.Add(CreateHand(testNo) as TEntity);
                        }
                        else if(checkEntity is  Leg)
                        {
                            entities.Add(CreateLeg(testNo) as TEntity);
                        }
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                    }
                    await context.DbSet<TEntity>().AddRangeAsync(entities);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected abstract Body CreateBody(int testNo);
        protected abstract Hand CreateHand(int testNo);
        protected abstract Leg CreateLeg(int testNo);
    }
}
