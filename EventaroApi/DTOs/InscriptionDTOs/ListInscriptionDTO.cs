using EventaroApi.Enums;

namespace EventaroApi.DTOs.InscriptionDTOs;

public class ListInscriptionDTO
{
    public int Id {get; set;}
    public DateTime DateInscription {get; set;}
    public StatusInscription StatusInscription {get; set;}

    public int EventId {get; set;}
    public string EventName {get; set;}

    public string UserId {get; set;}
    public string UserName {get; set;}
}