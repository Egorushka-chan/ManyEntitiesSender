using ManyEntitiesSender.Middleware;

namespace ManyEntitiesSender.Attributes
{
    /// <summary>
    /// Всё что делает атрибут - обозначает что запрос к <see cref="Endpoint"/> должен пройти через <see cref="CachingMiddleware"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheableAttribute : Attribute
    {

    }
}
