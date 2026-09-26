using System.Diagnostics;

namespace EventaroApi.DTOs.EventDTOs;

public class CreateEventDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public int OrganizationId { get; set; }
    public int EventTypeId { get; set; }
    public CreateUbicationDTO Ubication { get; set; } = new CreateUbicationDTO();
    public List<IFormFile> Images {get; set;} = new List<IFormFile>();
}