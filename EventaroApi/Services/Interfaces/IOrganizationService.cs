using EventaroApi.DTOs.OrganizationDTOs;
using EventaroApi.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace EventaroApi.Services.Interfaces
{
    public interface IOrganizationService
    {
        public Task<ResponseOrganizationDTO> CreateOrganization(CreateOrganizationDTO createOrganization);
        public Task<IEnumerable<ListOrganizationDTO>> GetAllOrganizations();
        public Task<ResponseOrganizationDTO> GetOrganizationById(int id);
        public Task<ResponseOrganizationDTO> PatchOrganization(int id, JsonPatchDocument<Organization> patchDoc);
        public Task<bool> DeleteOrganization(int id);
    }
}
