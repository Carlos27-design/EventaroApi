using EventaroApi.Enums;

namespace EventaroApi.DTOs.InscriptionDTOs;

public class UpdateInscriptionDTO
{
    public StatusInscription StatusInscription {get; set;}
    public DateTime DateInscription {get; set;}
}