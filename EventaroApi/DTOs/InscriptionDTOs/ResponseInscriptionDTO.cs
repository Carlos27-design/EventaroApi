using EventaroApi.DTOS.InscriptionDTOs;
using EventaroApi.Enums;

namespace EventaroApi.DTOs.InscriptionDTOs;

public class ResponseInscriptionDTO
{
    public int Id {get; set;}
    public DateTime DateInscription {get; set;}
    public StatusInscription StatusInscription {get; set;}

    public int EventId {get; set;}
    public string UserId {get; set;} = string.Empty;

    public EventSummaryDTO? Event {get; set;}
    public UserSummaryDTO? User {get; set;}
}