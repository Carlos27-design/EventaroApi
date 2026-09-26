using EventaroApi.DTOs.EventDTOs;

namespace EventaroApi.Services.Interfaces;

public interface IEventServices
{
    public Task<IEnumerable<ResponseEventDTO>> GetAllEvent(int? organizationId = null);
    public Task<ResponseEventDTO> GetEventById(int id);
    public Task<ResponseEventDTO> CreateEvent(CreateEventDTO createEvent);
    public Task<ResponseEventDTO> UpdateEvent(int id, UpdateEventDTO updateEvent);
    public Task<IEnumerable<ResponseEventDTO>> GetEventByOrganizationId(int organizationId);
    public Task<bool> DeleteEvent(int id);
}