using EventaroApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class Event : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public required DateTime EventStart { get; set; }
        [Required]
        public required DateTime EventEnd { get; set; }
        public StatusEvent StatusEvent { get; set; } = StatusEvent.Draft;
        //Foreign key
        public int OrganizationId { get; set; }
        public int EventTypeId { get; set; }
        public int UbicationId { get; set; }
        //Navigation properties
        public Organization? Organization { get; set; }
        public EventType? EventType { get; set; }
        public Ubication? Ubication { get; set; }

        public ICollection<EventImg> EventImgs { get; set; } = new List<EventImg>();
        public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
    }
}
