using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace ManyEntitiesSender.DAL.Interfaces
{
    public interface IPackageContext
    {
        DbSet<Package> Packages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        PackageContext Context { get; }
    }
}
