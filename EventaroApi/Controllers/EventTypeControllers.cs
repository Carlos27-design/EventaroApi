using EventaroApi.DTOs.EventTypeDTOs;
using EventaroApi.Entities;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EventaroApi.Controllers;

[ApiController]
[Route("api/eventType")]
public class EventTypeControllers: ControllerBase
{
    private readonly IEventTypeService _eventTypeService;


    public EventTypeControllers(IEventTypeService eventTypeService)
    {
        this._eventTypeService = eventTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseEventTypeDTO>>> GetAll()
    {
        try
        {
            var eventTypes = await _eventTypeService.GetAllTypeEvent();
            return Ok(eventTypes);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new {message = "Error al Cargar los tipos de eventos"});
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseEventTypeDTO>> GetById(int id)
    {
        try
        {
            var eventType = await _eventTypeService.GetTypeEventById(id);
            return Ok(eventType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {message = $"Error al cargar el tipo de evento con id {id}"});
        }
    }

    [HttpPost]
    public async Task<ActionResult<ResponseEventTypeDTO>> Create([FromBody] CreateEventTypeDTO createEventType)
    {
        try{
            var eventType = await _eventTypeService.CreateTypeEvent(createEventType);
            return Ok(eventType);
        }
        catch(Exception ex)
        {
            return NotFound(new {message = ex.Message});
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ResponseEventTypeDTO>> Patch(int id, JsonPatchDocument<EventType> patchDoc)
    {
        try
        {
            var eventType = await _eventTypeService.PatchTypeEvent(id, patchDoc);
            return Ok(eventType);
        }
        catch(Exception ex)
        {
            return NotFound(new {message=ex.Message});
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _eventTypeService.DeleteTypeEvent(id);
            return NoContent();
        }
        catch(Exception ex)
        {
            return NotFound(new {message = ex.Message});
        }
    }
}
