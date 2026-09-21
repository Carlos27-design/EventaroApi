using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class Organization : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
