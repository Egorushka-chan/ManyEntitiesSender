using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace ManyEntitiesSender.DAL.Interfaces
{
    public interface IRedisProvider
    {
        IConnectionMultiplexer GetConnectionMultiplexer();
    }
}
