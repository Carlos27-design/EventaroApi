using AutoMapper;
using EventaroApi.Data;
using EventaroApi.DTOs.EventTypeDTOs;
using EventaroApi.Entities;
using EventaroApi.Enums;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EventaroApi.Services;

public class EventTypeServices : IEventTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EventTypeServices(ApplicationDbContext context, IMapper mapper)
    {
        this._context = context;
        this._mapper = mapper;

    }

    public async Task<ResponseEventTypeDTO> CreateTypeEvent(CreateEventTypeDTO createEventType)
    {
        var typeEvent = await _context.EventTypes.FirstOrDefaultAsync(et => et.Name == createEventType.Name);
        if (typeEvent != null)
        {
            throw new Exception("El tipo de evento ya existe");
        }

        var newTypeEvent = _mapper.Map<EventType>(createEventType);

        _context.EventTypes.Add(newTypeEvent);

        await _context.SaveChangesAsync();

        return _mapper.Map<ResponseEventTypeDTO>(newTypeEvent);
    }

    public async Task<bool> DeleteTypeEvent(int id)
    {
        var typeEvent = await _context.EventTypes.FindAsync(id);
        if (typeEvent == null)
        {
            throw new Exception($"No existe un tipo de evento con el id {id}");
        }

        typeEvent.Status = StatusBase.Delete;
        typeEvent.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ResponseEventTypeDTO>> GetAllTypeEvent()
    {
        var eventTypes = await _context.EventTypes.Where(et => et.Status == StatusBase.Active).ToListAsync();
        if (eventTypes == null)
        {
            throw new Exception("No Existe ningun tipo de evento");
        }

        return _mapper.Map<IEnumerable<ResponseEventTypeDTO>>(eventTypes);
    }

    public async Task<ResponseEventTypeDTO> GetTypeEventById(int id)
    {
        var eventType = await _context.EventTypes.FirstOrDefaultAsync(et => et.Id == id && et.Status == StatusBase.Active);
        if (eventType == null)
        {
            throw new Exception($"No existe un tipo de evento con id {id}");
        }

        return _mapper.Map<ResponseEventTypeDTO>(eventType);
    }

    public async Task<ResponseEventTypeDTO> PatchTypeEvent(int id, JsonPatchDocument<EventType> patchDoc)
    {
        var eventType = await _context.EventTypes.FindAsync(id);
        if (eventType == null || eventType.Status == StatusBase.Delete)
        {
            throw new Exception($"No existe el tipo de evento con id {id}");
        }

        patchDoc.ApplyTo(eventType);

        eventType.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<ResponseEventTypeDTO>(eventType);
    }

}
