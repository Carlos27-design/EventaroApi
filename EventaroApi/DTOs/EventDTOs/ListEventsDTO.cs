namespace EventaroApi.DTOs.EventDTOs;

public class ListEventsDTO
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public DateTime EventStart {get; set;}
    public DateTime EventFin {get; set;}
    public string OrganizationName {get; set;}
    public string EventTypeName {get; set;}
    public string UbicationName {get; set;}
    public List<string>? ImageUrls {get; set;}
}
