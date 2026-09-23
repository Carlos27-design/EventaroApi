using EventaroApi.DTOs.OrganizationDTOs;
using EventaroApi.Entities;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EventaroApi.Controllers
{
    [ApiController]
    [Route("api/organization")]
    public class OrganizationControllers : ControllerBase
    {
        private readonly IOrganizationService _organizationServices;

        public OrganizationControllers(IOrganizationService organizationServices)
        {
            this._organizationServices = organizationServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseOrganizationDTO>>> GetAllOrganization()
        {
            try
            {
                var organizations = await _organizationServices.GetAllOrganizations();
                return Ok(organizations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrio un error al obtener las organizaciones" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseOrganizationDTO>> GetById(int id)
        {
            try
            {
                var organization = await _organizationServices.GetOrganizationById(id);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrio un error al obtener la organización" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseOrganizationDTO>> CreateOrganization([FromBody] CreateOrganizationDTO createOrganization)
        {
            try
            {
                var organization = await _organizationServices.CreateOrganization(createOrganization);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrio un error al crear la organización" });
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ResponseOrganizationDTO>> PatchOrganization(int id, [FromBody] JsonPatchDocument<Organization> patchDocument)
        {
            try
            {
                var organization = await _organizationServices.PatchOrganization(id, patchDocument);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrio un error al actualizar la organización" });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            try
            {
                await _organizationServices.DeleteOrganization(id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
