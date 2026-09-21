using EventaroApi.Enums;

namespace EventaroApi.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public StatusBase Status { get; set; } = StatusBase.Active;
    }
}
