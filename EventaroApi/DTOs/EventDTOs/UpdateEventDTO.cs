namespace EventaroApi.DTOs.EventDTOs;

public class UpdateEventDTO
{
    public string Name {get; set;}
    public string Description {get; set;}
    public DateTime EventStart {get; set;}
    public DateTime EventEnd {get; set;}
    public int OrganizationId {get; set;}
    public int EventTypeId {get; set;}
    public UpdateUbicationDTO Ubication { get; set; } = new UpdateUbicationDTO();
    public List<IFormFile> Images {get; set;} = new List<IFormFile>();
}