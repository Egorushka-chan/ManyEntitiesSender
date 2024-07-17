namespace ManyEntitiesSender.Models
{
    /// <summary>
    /// Статический класс, имеющий объекты для блокировки таблицы 
    /// </summary>
    public static class CacheLockers
    {
        public static object BodyLocker = new object();
        public static object HandLocker = new object();
    }
}
