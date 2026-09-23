using EventaroApi.DTOs.EventTypeDTOs;
using EventaroApi.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace EventaroApi.Services.Interfaces;

public interface IEventTypeService
{
    public Task<ResponseEventTypeDTO> CreateTypeEvent(CreateEventTypeDTO createEventType);
    public Task<ResponseEventTypeDTO> GetTypeEventById(int id);
    public Task<IEnumerable<ResponseEventTypeDTO>> GetAllTypeEvent();
    public Task<ResponseEventTypeDTO> PatchTypeEvent(int id, JsonPatchDocument<EventType> patchDoc);
    public Task<bool> DeleteTypeEvent(int id);
}
