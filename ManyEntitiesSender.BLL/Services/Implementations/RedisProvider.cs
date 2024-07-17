using System.Text;
using System.Security.Cryptography;

using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Entities;
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

            if (luaScriptBodies is null) {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "internalFiles");
                luaScriptBodies = new Dictionary<string, string> {
                    { "body", File.ReadAllText(Path.Combine(path, "tryGetBodies.lua")) },
                    { "hand", File.ReadAllText(Path.Combine(path, "tryGetHands.lua")) },
                    { "leg", File.ReadAllText(Path.Combine(path, "tryGetLegs.lua")) }
                };
            };
        }

        protected RedisSettings settings { get; set; }

        private static IConnectionMultiplexer __connectionMultiplexer;
        private static Dictionary<string, string> luaScriptBodies;

        /// <summary>
        /// Возвращает объект из singleton, или создаёт новый, если объект ещё не создан
        /// </summary>
        /// <returns></returns>
        public IConnectionMultiplexer GetConnectionMultiplexer()
        {
            if(__connectionMultiplexer is null)
            {
                __connectionMultiplexer = ConnectionMultiplexer.Connect(settings.Configuration + ",protocol=resp3");
            }
            return __connectionMultiplexer;
        }

        protected IDatabase GetDatabase()
        {
            return GetConnectionMultiplexer().GetDatabase(settings.DatabaseID);
        }

        private byte[] GetMD5(string value)
        {
            byte[] mightinessBytes = Encoding.Default.GetBytes(value);
            byte[] mightinessHashBytes = MD5.HashData(mightinessBytes);
            return mightinessHashBytes;
        }

        private Task<long> IncreaseCounter(string table, string value)
        {
            return GetDatabase().StringIncrementAsync(new RedisKey($"counter:{table}:{value}"), 1);
        }

        private async Task<long> GetCounter(string table, string value)
        {
            RedisValue counter = await GetDatabase().StringGetAsync(new RedisKey($"counter:{table}:{value}"));
            if (counter.IsNullOrEmpty)
                return 0;

            counter.TryParse(out long result);
            return result;
        }

        /// <inheritdoc/>
        public Task AppendList<TEntity>(List<TEntity> list) where TEntity: class, IEntity
        {
            // Проверка что тут реализован тип, который будет использоваться
            Type type = typeof(TEntity);
            Type[] allowedTypes = { typeof(Body), typeof(Hand), typeof(Leg) };
            if (!allowedTypes.Contains(type))
                throw new ArgumentException($"Type {type.Name} can't be inserted inside Redis (Not Implemented)");

            // Зачем тут этот делегат? Чтобы не просчитывать выбор типа каждый раз в блоке foreach далее
            Func<List<HashEntry>, IEntity, string> appendFields;
            if (type == typeof(Body)) {
                appendFields = (List<HashEntry> fields, IEntity entity) => {
                    HashEntry mightiness = new HashEntry(new RedisValue("mightiness"), new RedisValue(((Body)entity).Mightiness));

                    RedisValue hashValue = RedisValue.Unbox(GetMD5(((Body)entity).Mightiness));
                    HashEntry mightinessHash = new HashEntry(new RedisValue("mightinessIndex"), hashValue);

                    fields.Add(mightiness);
                    fields.Add(mightinessHash);

                    return ((Body)entity).Mightiness;
                };
            }
            else if(type == typeof(Hand)) {
                appendFields = (List<HashEntry> fields, IEntity entity) => {
                    HashEntry state = new HashEntry(new RedisValue("state"), new RedisValue(((Hand)entity).State));

                    RedisValue hashValue = RedisValue.Unbox(GetMD5(((Hand)entity).State));
                    HashEntry stateHash = new HashEntry(new RedisValue("stateIndex"), hashValue);

                    fields.Add(state);
                    fields.Add(stateHash);

                    return ((Hand)entity).State;
                };
            }
            else if (type == typeof(Leg)) {
                appendFields = (List<HashEntry> fields, IEntity entity) => {
                    HashEntry state = new HashEntry(new RedisValue("state"), new RedisValue(((Leg)entity).State));

                    RedisValue hashValue = RedisValue.Unbox(GetMD5(((Leg)entity).State));
                    HashEntry stateHash = new HashEntry(new RedisValue("stateIndex"), hashValue);

                    fields.Add(state);
                    fields.Add(stateHash);

                    return ((Leg)entity).State;
                };
            }
            else {
                throw new ArgumentException($"Type {type.Name} can't be inserted inside Redis (Not Implemented)");
            }
            
            int entityCount = list.Count;
            foreach (TEntity element in list) {
                List<HashEntry> fields = new();
                string value = appendFields.Invoke(fields, element);

                // Все запросы делаются в пайплайне, и не ожидаются. Ожидаться они будут задачей далее.
                GetDatabase().HashSetAsync(new RedisKey($"{nameof(Body)}:{value}:{element.ID}"), fields.ToArray())
                    .ContinueWith((antecedent) => {
                        IncreaseCounter($"{nameof(Body)}", $"{value}");
                        entityCount--;
                    });

            }

            // Эта задача нужна для возврата того, что можно ожидать
            Task awaitedTask = Task.Run(() => {
                bool succeeded = false;
                while (!succeeded) {
                    if (entityCount >= 0) {
                        succeeded = true;
                    }
                }
            });

            return awaitedTask;
        }

        public async Task<List<Body>?> TryGetBodies(RedisFilterOptions filterOptions)
        {
            
            if (filterOptions.PropertyFilter != null) {
                var script = LuaScript.Prepare(luaScriptBodies["body"]);
                var result = GetDatabase().ScriptEvaluate(script, new {});
            }
            else {
                
            }

            throw new NotImplementedException();
        }

        public async Task<List<Body>?> TryGetHands(RedisFilterOptions filterOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Body>?> TryGetLegs(RedisFilterOptions filterOptions)
        {
            throw new NotImplementedException();
        }
    }
}
