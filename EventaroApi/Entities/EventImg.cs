using System.ComponentModel.DataAnnotations;

namespace EventaroApi.Entities
{
    public class EventImg: BaseEntity
    {
        [Required]
        public required string ImgUrl { get; set; }

        //Foreign Key
        public int EventId { get; set; }

        //Navigation Property
        public Event? Event { get; set; }
    }
}
