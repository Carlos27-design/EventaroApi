using AutoMapper;
using EventaroApi.Data;
using EventaroApi.DTOs.InscriptionDTOs;
using EventaroApi.Entities;
using EventaroApi.Enums;
using EventaroApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventaroApi.Services;

public class InscriptionServices : IInscriptionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InscriptionServices(ApplicationDbContext context, IMapper mapper)
    {
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<ResponseInscriptionDTO> Create(CreateInscriptionDTO createInscription, string userId)
    {
        var eventExists = await _context.Events.AnyAsync(e => e.Id == createInscription.EventId);

        if (!eventExists)
        {
            throw new KeyNotFoundException($"El evento con ID {createInscription.EventId} no existe");
        }

        var alreadyInscription = await _context.Inscriptions.AnyAsync(i => i.EventId == createInscription.EventId && i.UserId == userId);

        if (alreadyInscription)
        {
            throw new InvalidOperationException("El usuario ya esta inscrito en este evento");
        }

        var inscription = _mapper.Map<Inscription>(createInscription);
        inscription.UserId = userId;
        _context.Inscriptions.Add(inscription);
        await _context.SaveChangesAsync();

        return _mapper.Map<ResponseInscriptionDTO>(inscription);
    }

    public async Task<bool> Delete(int id)
    {
        var inscription = await _context.Inscriptions.FindAsync(id);
        if(inscription == null)
        {
            return false;
        }

        inscription.Status = StatusBase.Delete;
        await _context.SaveChangesAsync();

        return true;
    }

    public Task<IEnumerable<ResponseInscriptionDTO>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ResponseInscriptionDTO>> GetByEventId(int eventId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseInscriptionDTO> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ResponseInscriptionDTO>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseInscriptionDTO> Update(int id, UpdateInscriptionDTO updateInscription)
    {
        throw new NotImplementedException();
    }
}