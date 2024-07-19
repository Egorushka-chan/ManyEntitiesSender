using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

namespace ManyEntitiesSender.BLL.Services.Abstractions
{
    /// <summary>
    /// Реализация этого интерфейса должна проверять существование объекта в Redis, иначе возвращать объект из базы данных
    /// </summary>
    public interface IPackageGetter
    {
        /// <summary>
        /// Возвращает информацию пакетами из базы
        /// </summary>
        /// <remarks>
        /// Текущая реализация позволяет кэшировать элементы типов: <see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/>
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Body"/>, <see cref="Hand"/>, <see cref="Leg"/></typeparam>
        /// <exception cref="ArgumentException"></exception>
        IAsyncEnumerable<List<TEntity>> GetPackageAsync<TEntity>(EntityFilterOptions filterOptions) where TEntity : class, IEntity, new();
    }
}
