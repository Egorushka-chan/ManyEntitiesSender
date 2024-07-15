using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Entities;

namespace ManyEntitiesSender.BLL.Services.Implementations
{
    public class CacheableGetter : ICacheableGetter
    {
        public IAsyncEnumerable<List<Package>> GetAllPackagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
