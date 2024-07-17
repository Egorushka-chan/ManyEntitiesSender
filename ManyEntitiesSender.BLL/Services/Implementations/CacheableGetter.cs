using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Entities;

namespace ManyEntitiesSender.BLL.Services.Implementations
{
    public class CacheableGetter : ICacheableGetter
    {
        public IAsyncEnumerable<List<Body>> GetBodiesAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<List<Hand>> GetHandsAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<List<Leg>> GetLegsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
