using EventaroApi.DTOs.InscriptionDTOs;

namespace EventaroApi.Services.Interfaces;

public interface IInscriptionService
{
    Task<ResponseInscriptionDTO> Create(CreateInscriptionDTO createInscription, string userId);
    Task<ResponseInscriptionDTO> Update(int id, UpdateInscriptionDTO updateInscription);
    Task<ResponseInscriptionDTO> GetById(int id);
    Task<IEnumerable<ResponseInscriptionDTO>> GetAll();
    Task<IEnumerable<ResponseInscriptionDTO>> GetByUserId(string userId);
    Task<IEnumerable<ResponseInscriptionDTO>> GetByEventId(int eventId);
    Task<bool> Delete(int id);

}