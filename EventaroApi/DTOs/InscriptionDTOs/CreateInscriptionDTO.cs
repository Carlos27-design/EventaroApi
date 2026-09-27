namespace EventaroApi.DTOs.InscriptionDTOs;

public class CreateInscriptionDTO
{
    public DateTime DateInscription { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; } = string.Empty;
}
