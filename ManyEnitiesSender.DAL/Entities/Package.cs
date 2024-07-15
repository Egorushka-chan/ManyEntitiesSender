using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyEntitiesSender.DAL.Entities
{
    public partial class Package
    {
        [Key]
        public int ID { get; set; }
        public string? Key { get; set; }
        public string? Data { get; set; }
    }
}
