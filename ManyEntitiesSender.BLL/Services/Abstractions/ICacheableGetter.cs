using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.DAL.Entities;

namespace ManyEntitiesSender.BLL.Services.Abstractions
{
    public interface ICacheableGetter
    {
        IAsyncEnumerable<List<Package>> GetAllPackagesAsync();
    }
}
