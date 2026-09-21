using EventaroApi.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(150)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(150)]
        public required string LastName { get; set; }
        public StatusBase Status { get; set; } = StatusBase.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        //Foreign Key
        public int? OrganizationId { get; set; }
        
        //Navigation Property
        public Organization? Organization { get; set; }

        public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
    }
    
}
