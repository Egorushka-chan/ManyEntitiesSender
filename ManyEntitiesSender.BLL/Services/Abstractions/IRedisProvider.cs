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
        /// <param name="array">Элементы, которые будут отправлены в кэш</param>
        /// <returns><see cref="Task"/> для ожидания</returns>
        /// <exception cref="ArgumentException">Возникает при подаче неправильного типа</exception>
        Task AppendListAsync<TEntity>(TEntity[] array) where TEntity : class, IEntity;
        /// <summary>
        /// Забирает кэшированные элементы из Redis
        /// </summary>
        /// <remarks>
        /// Текущая реализация позволяет кэшировать элементы типов: <see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/>
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/></typeparam>
        /// <param name="filterValue">Значение по которому фильтруем</param>
        /// <returns>Если по такому значению в Redis нет ничего, то не вернёт ничего. Иначе будет возвращать пакеты размером, определённым в <see cref="PackageSettings"/> </returns>
        /// <exception cref="ArgumentException"></exception>
        IAsyncEnumerable<List<TEntity>> TryGetAsync<TEntity>(string? filterValue = null) where TEntity : class, IEntity, new();
    }
}
