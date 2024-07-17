using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyEntitiesSender.BLL.Models.Requests
{
    public class PackageRequest
    {
        public string? Table {  get; set; }
        public string? Filter { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
