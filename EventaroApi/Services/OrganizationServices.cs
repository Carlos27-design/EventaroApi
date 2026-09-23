using AutoMapper;
using EventaroApi.Data;
using EventaroApi.DTOs.OrganizationDTOs;
using EventaroApi.Entities;
using EventaroApi.Enums;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EventaroApi.Services
{
    public class OrganizationServices : IOrganizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationServices(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<ResponseOrganizationDTO> CreateOrganization(CreateOrganizationDTO createOrganization)
        {
            var organizationExists = _context.Organizations.FirstOrDefault(o => o.Name == createOrganization.Name);

            if(organizationExists != null)
            {
                throw new Exception("Ya existe una Organización con ese Nombre");
            }

            var newOrganization = _mapper.Map<Organization>(createOrganization);

            _context.Organizations.Add(newOrganization);
            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseOrganizationDTO>(newOrganization);
        }

        public async Task<bool> DeleteOrganization(int id)
        {
            //Verificar si existe la Organization
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                throw new Exception($"No existe la organization con el id {id}");
            }

            organization.Status = StatusBase.Delete;
            organization.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ListOrganizationDTO>> GetAllOrganizations()
        {
            var organizations = await _context.Organizations.Where(o => o.Status == StatusBase.Active).ToListAsync();

            if(organizations == null)
            {
                throw new Exception("No existe ninguna organización");
            }

            return _mapper.Map<IEnumerable<ListOrganizationDTO>>(organizations);
        }

        public async Task<ResponseOrganizationDTO> GetOrganizationById(int id)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == id && o.Status == StatusBase.Active);

            if(organization == null)
            {
                throw new Exception($"No existe la organización con id {id}");
            }

            return _mapper.Map<ResponseOrganizationDTO>(organization);
        }

        public async Task<ResponseOrganizationDTO> PatchOrganization(int id, JsonPatchDocument<Organization> patchDoc)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if(organization == null || organization.Status == StatusBase.Delete)
            {
                throw new Exception($"No existe la organization con el id {id}");
            }

            patchDoc.ApplyTo(organization);

            organization.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseOrganizationDTO>(organization);
        }
    }
}
