using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.DAL.Entities;

using StackExchange.Redis;

namespace ManyEntitiesSender.DAL.Interfaces
{
    public interface IRedisProvider
    {
        IConnectionMultiplexer GetConnectionMultiplexer();
        /// <summary>
        /// Кэширует элементы в Redis
        /// </summary>
        /// <remarks>
        /// Текущая реализация позволяет кэшировать элементы типов: <see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/>
        /// </remarks>
        /// <typeparam name="TEntity">Должен соответствовать одному из типов: <see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/></typeparam>
        /// <param name="list">Элементы, которые будут отправлены в кэш</param>
        /// <returns><see cref="Task"/> для ожидания</returns>
        /// <exception cref="ArgumentException">Возникает при подаче неправильного типа</exception>
        Task AppendList<TEntity>(List<TEntity> list) where TEntity : class, IEntity;
        Task<List<Body>?> TryGetBodies(RedisFilterOptions filterOptions);
        Task<List<Body>?> TryGetHands(RedisFilterOptions filterOptions);
        Task<List<Body>?> TryGetLegs(RedisFilterOptions filterOptions);
    }
}
