using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class EventType: BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
