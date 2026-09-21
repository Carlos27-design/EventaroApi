using EventaroApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class Inscription : BaseEntity
    {
        [Required]
        public required DateTime DateInscription { get; set; }
        public StatusInscription StatusInscription { get; set; } = StatusInscription.Pending;

        //Foreign Key
        public required int EventId { get; set; }
        public required string UserId { get; set; }

        //Navigation Property
        public required Event Event { get; set; }
        public required User User { get; set; }
    }
}
