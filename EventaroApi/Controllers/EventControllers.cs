using EventaroApi.DTOs.EventDTOs;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventaroApi.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventControllers : ControllerBase
    {
        private readonly IEventServices _eventService;

        public EventControllers(IEventServices eventService)
        {
            this._eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseEventDTO>>> GetAllEvent([FromQuery] int? organizationId)
        {
            try
            {
                var events = await _eventService.GetAllEvent(organizationId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    message = "Ocurrio un error al obtener lo eventos",
                    error = ex.Message
                });
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseEventDTO>> GetByIdEvent(int id)
        {
            try
            {
                var eventEntity = await _eventService.GetEventById(id);
                return Ok(eventEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    message = "Ocurrio un error al obtener el evento",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEventDTO>> Create([FromForm] CreateEventDTO createEvent)
        {
            try
            {
                var eventEntity = await _eventService.CreateEvent(createEvent);
                return Ok(eventEntity);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("my-organization")]
        [Authorize]
        public async Task<ActionResult> GetMyOrganization()
        {
            try
            {
                var orgIdClaim = User.FindFirst("OrganizationId")?.Value;

                if (string.IsNullOrEmpty(orgIdClaim) || int.TryParse(orgIdClaim, out int organizationId))
                {
                    return Unauthorized(new { message = "No se pudo identificar tu rol en el token" });
                }

                var events = await _eventService.GetEventByOrganizationId(organizationId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al cargar los eventos",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseEventDTO>> update(int id, [FromForm] UpdateEventDTO updateEvent) 
        {
            try
            {
                var eventUpdate = await _eventService.UpdateEvent(id, updateEvent);
                return Ok(eventUpdate);
            }
            catch(Exception ex) 
            {
                return NotFound(new { message = ex.Message });                
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                await _eventService.DeleteEvent(id);
                return Ok(new { message = "Evento eliminado Exitosamente" });
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
