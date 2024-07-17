using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyEntitiesSender.BLL.Models
{
    public class RedisFilterOptions
    {
        public string? PropertyFilter { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
