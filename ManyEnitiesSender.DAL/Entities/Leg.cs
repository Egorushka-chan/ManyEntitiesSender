using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using ManyEntitiesSender.DAL.Interfaces;

namespace ManyEntitiesSender.DAL.Entities
{
    public partial class Leg : IEntity
    {
        [Key]
        [JsonPropertyName("id")]
        public long ID  { get; set; }
        [JsonPropertyName("state")]
        public string State {get; set; }
    }
}
