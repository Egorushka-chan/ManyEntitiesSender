using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.DAL.Interfaces;

namespace ManyEntitiesSender.DAL.Entities
{
    public partial class Body : IEntity
    {
        [Key]
        public long ID { get; set; }
        public string Mightiness {get; set; }
    }
}
