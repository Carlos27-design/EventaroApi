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

    public async Task<IEnumerable<ResponseInscriptionDTO>> GetAll()
    {
        var inscription = await _context.Inscriptions
            .Include(i => i.Event)
            .Include(i => i.User)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ResponseInscriptionDTO>>(inscription);
    }

    public async Task<IEnumerable<ResponseInscriptionDTO>> GetByEventId(int eventId)
    {
        var inscriptionEvent = await _context.Inscriptions
            .Where(i => i.EventId == eventId)
            .Include(i => i.Event)
            .Include(i => i.User)
            .ToListAsync();

        if(inscriptionEvent == null)
        {
            throw new Exception("No se encuentran Inscriptiones para este evento");
        }
        
        return _mapper.Map<IEnumerable<ResponseInscriptionDTO>>(inscriptionEvent);
    }

    public async Task<ResponseInscriptionDTO> GetById(int id)
    {
        var inscription = await _context.Inscriptions
            .Include(i => i.Event)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);

        if(inscription == null)
        {
            throw new Exception($"La Inscripción con ID {id} no existe");
        }

        return _mapper.Map<ResponseInscriptionDTO>(inscription);
    }

    public async Task<IEnumerable<ResponseInscriptionDTO>> GetByUserId(string userId)
    {
        var inscriptions = await _context.Inscriptions
            .Where(i => i.UserId == userId)
            .Include(i => i.Event)
            .Include(i => i.User)
            .ToListAsync();

        if(inscriptions.Any())
        {
            return Enumerable.Empty<ResponseInscriptionDTO>();
        }

        return _mapper.Map<IEnumerable<ResponseInscriptionDTO>>(inscriptions);    
    }

    public async Task<ResponseInscriptionDTO> Update(int id, UpdateInscriptionDTO updateInscription)
    {
        var inscription = await _context.Inscriptions.FindAsync(id);

        if(inscription == null)
        {
            throw new Exception($"La Inscripción con ID {id} no existe");
        }

        inscription.StatusInscription = StatusInscription.Confirmed;
        inscription.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        return _mapper.Map<ResponseInscriptionDTO>(inscription);
    }
}