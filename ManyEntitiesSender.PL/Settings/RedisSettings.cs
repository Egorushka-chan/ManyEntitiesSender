namespace ManyEntitiesSender.BLL.Settings
{
    public class RedisSettings
    {
        public string Configuration { get; set; } = "localhost";
        public string InstanceName { get; set; } = "local";
        public int DatabaseID { get; set; } = 0;
    }
}
