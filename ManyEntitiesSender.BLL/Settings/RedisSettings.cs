using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyEntitiesSender.BLL.Settings
{
    public class RedisSettings
    {
        public string Configuration { get; set; } = "localhost";
        public string InstanceName { get; set; } = "local";
        public int DatabaseID { get; set; } = 0;
    }
}
