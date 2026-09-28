using System.Security.Claims;
using EventaroApi.DTOs.InscriptionDTOs;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventaroApi.Controllers;

[ApiController]
[Route("api/inscriptions")]
public class InscriptionControllers : ControllerBase
{
    private readonly IInscriptionService _inscriptionService;

    public InscriptionControllers(IInscriptionService inscriptionService)
    {
        this._inscriptionService = inscriptionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseInscriptionDTO>>> GetAll()
    {
        try{
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new {message = "No se pudo identificar al usuario"});
            }

            var result = await _inscriptionService.GetAll();
            return Ok(result);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las inscripciones", error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseInscriptionDTO>> GetById(int id){
        try
        {
            var inscription = await _inscriptionService.GetById(id);
            return Ok(inscription);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener la inscripción", error = ex.Message });
        }
    }

    [HttpGet("event/{eventId:int}")]
    public async Task<ActionResult<IEnumerable<ResponseInscriptionDTO>>> GetByEventId(int eventId)
    {
        try{
            var result = await _inscriptionService.GetByEventId(eventId);
            return Ok(result);
        }
        catch(Exception ex)
        {
              return StatusCode(500, new { message = "Error al obtener la inscripción", error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize]
      public async Task<ActionResult<ResponseInscriptionDTO>> Create([FromBody] CreateInscriptionDTO dto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "No se pudo identificar al usuario" });
            }

            var result = await _inscriptionService.Create(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear la inscripción", error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ResponseInscriptionDTO>> Update(int id, [FromBody] UpdateInscriptionDTO dto)
    {
        try
        {
            var result = await _inscriptionService.Update(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar la inscripción", error = ex.Message });
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _inscriptionService.Delete(id);
            
            if (!success)
            {
                return NotFound(new { message = "La inscripción no existe" });
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar la inscripción", error = ex.Message });
        }
    }


}