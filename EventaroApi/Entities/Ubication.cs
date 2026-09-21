using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;


namespace EventaroApi.Entities
{
    public class Ubication : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public required string Address { get; set; }
        [Required]
        public required Point Location { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
