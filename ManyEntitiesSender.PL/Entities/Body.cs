using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using ManyEntitiesSender.DAL.Interfaces;

namespace ManyEntitiesSender.DAL.Entities
{
    public partial class Body : IEntity
    {
        [Key]
        [JsonPropertyName("id")]
        public long ID { get; set; }
        [JsonPropertyName("mightiness")]
        public string Mightiness {get; set; }
    }
}
