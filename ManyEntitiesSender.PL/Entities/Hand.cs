using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using ManyEntitiesSender.DAL.Interfaces;

namespace ManyEntitiesSender.DAL.Entities
{
    public partial class Hand : IEntity
    {
        [Key]
        [JsonPropertyName("id")]
        public long ID { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
