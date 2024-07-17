using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace ManyEntitiesSender.DAL.Implementations
{
    public class RedisProvider : IRedisProvider
    {
        public RedisProvider(IOptions<RedisSettings> settings)
        {
            this.settings = settings.Value;
        }

        private RedisSettings settings { get; set; }
        private static IConnectionMultiplexer __connectionMultiplexer;

        /// <summary>
        /// Возвращает объект из singleton, или создаёт новый, если объект ещё не создан
        /// </summary>
        /// <returns></returns>
        public IConnectionMultiplexer GetConnectionMultiplexer()
        {
            if(__connectionMultiplexer is null)
            {
                __connectionMultiplexer = ConnectionMultiplexer.Connect(settings.Configuration);
            }
            return __connectionMultiplexer;
        }

        public IDatabase GetDatabase()
        {
            return GetConnectionMultiplexer().GetDatabase();
        }

        public string GetString(string key)
        {
            RedisValue result = GetDatabase().StringGet(new RedisKey(key));
            return result.ToString();
        }
    }
}
