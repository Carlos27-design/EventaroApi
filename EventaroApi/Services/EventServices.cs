using AutoMapper;
using EventaroApi.Data;
using EventaroApi.DTOs.EventDTOs;
using EventaroApi.DTOs.OrganizationDTOs;
using EventaroApi.Entities;
using EventaroApi.Enums;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NetTopologySuite.Geometries;

namespace EventaroApi.Services
{
    public class EventServices : IEventServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivo _almacenadorArchivo;

        public EventServices(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivo almacenadorArchivo)
        {
            this._context = context;
            this._mapper = mapper;
            this._almacenadorArchivo = almacenadorArchivo;
        }

        public async Task<ResponseEventDTO> CreateEvent(CreateEventDTO createEvent)
        {
            var eventExist = await _context.Events.FirstOrDefaultAsync(e => e.Name == createEvent.Name && e.OrganizationId == createEvent.OrganizationId);

            if(eventExist != null)
            {
                throw new Exception("Ya existe un evento con este nombre para esta organización");
            }

            var newUbication = new Ubication
            {
                Address = createEvent.Ubication.Address,
                Location = new Point(createEvent.Ubication.Longitude, createEvent.Ubication.Latitude)
                {
                    SRID = 4326
                }
            };

            _context.Ubications.Add(newUbication);
            await _context.SaveChangesAsync();

            var newEvent = _mapper.Map<Event>(createEvent);
            newEvent.UbicationId = newUbication.Id;
            newEvent.EventImgs = new List<EventImg>();


            if(createEvent.Images != null && createEvent.Images.Any())
            {
                foreach(var image in createEvent.Images)
                {
                    if(image.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await image.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();

                        string imageUrl = await _almacenadorArchivo.GuardarArchivo(
                            contenido: fileBytes,
                            extension: Path.GetExtension(image.FileName),
                            contenedor: "eventos",
                            contentType: image.ContentType
                        );

                        newEvent.EventImgs.Add(new EventImg { ImgUrl = imageUrl });
                    }
                }
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            var createdEvent = await _context.Events
                .Include(e => e.Organization)
                .Include(e => e.EventType)
                .Include(e => e.Ubication)
                .Include(e => e.EventImgs)
                .FirstAsync(e => e.Id == newEvent.Id);

            return _mapper.Map<ResponseEventDTO>(createdEvent);
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.EventImgs)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(eventToDelete == null)
            {
                throw new Exception("Evento no encontrado");
            }

            eventToDelete.Status = StatusBase.Delete;
            eventToDelete.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ResponseEventDTO>> GetAllEvent(int? organizationId = null)
        {
            var query = _context.Events.AsQueryable();

            if (organizationId.HasValue)
            {
                query = query.Where(e => e.OrganizationId == organizationId.Value);
            }

            var events = await query
                .Include(e => e.Organization)
                .Include(e => e.EventType)
                .Include(e => e.Ubication)
                .Include(e => e.EventImgs)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseEventDTO>>(events);
        }

        public async Task<ResponseEventDTO> GetEventById(int id)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Organization)
                .Include(e => e.EventType)
                .Include(e => e.Ubication)
                .Include(e => e.EventImgs)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(eventEntity == null)
            {
                throw new Exception("Evento no encontrado");
            }

            return _mapper.Map<ResponseEventDTO>(eventEntity);
        }

        public async Task<IEnumerable<ResponseEventDTO>> GetEventByOrganizationId(int organizationId)
        {
            var events = await _context.Events
                .Where(e => e.OrganizationId == organizationId)
                .Include(e => e.Organization)
                .Include(e => e.EventType)
                .Include(e => e.Ubication)
                .Include(e => e.EventImgs)
                .ToListAsync();

            if(events == null)
            {
                throw new Exception("Eventos no encotrados");
            }

            return _mapper.Map<IEnumerable<ResponseEventDTO>>(events);
        }

        public async Task<ResponseEventDTO> UpdateEvent(int id, UpdateEventDTO updateEvent)
        {
            var eventToUpdate = await _context.Events
                .Include(e => e.Ubication)
                .Include(e => e.EventImgs)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(eventToUpdate == null)
            {
                throw new Exception("Evento no encontrado o ya fue eliminado");
            }

            var eventTypeExist = await _context.EventTypes
                .AnyAsync(et => et.Id == updateEvent.EventTypeId);

            if (!eventTypeExist)
            {
                throw new Exception($"El EventTypeId {updateEvent.EventTypeId} no existe");
            }

            var eventExist = await _context.Events.FirstOrDefaultAsync(e => e.Name == updateEvent.Name 
                                                                         && e.OrganizationId == eventToUpdate.OrganizationId 
                                                                         && e.Id != id);

            if(eventExist != null)
            {
                throw new Exception("Ya existe un evento con este nombre para esta organización");
            }

            eventToUpdate.Ubication.Address = updateEvent.Ubication.Address;
            eventToUpdate.Ubication.Location = new Point(
                updateEvent.Ubication.Longitude,
                updateEvent.Ubication.Latitude
            )
            {
                SRID = 4326
            };

            eventToUpdate.Ubication.UpdatedAt = DateTime.UtcNow;

            _mapper.Map(updateEvent, eventToUpdate);
            eventToUpdate.UpdatedAt = DateTime.UtcNow;

            if(updateEvent.Images != null && updateEvent.Images.Any())
            {
                for(int i = 0; i < updateEvent.Images.Count; i++)
                {
                    var newImage = updateEvent.Images[i];
                    if(newImage.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await newImage.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();

                        if(i < eventToUpdate.EventImgs.Count)
                        {
                            var existingImg = eventToUpdate.EventImgs.ElementAt(i);
                            string newUrl = await _almacenadorArchivo.EditarArchivo(
                                contenido: fileBytes,
                                extension: Path.GetExtension(newImage.FileName),
                                contenedor: "eventos",
                                ruta: existingImg.ImgUrl,
                                contentType: newImage.ContentType
                            );

                            existingImg.ImgUrl = newUrl;
                        }
                        else
                        {
                            string imagenUrl = await _almacenadorArchivo.GuardarArchivo(
                                contenido: fileBytes,
                                extension: Path.GetExtension(newImage.FileName),
                                contenedor: "eventos",
                                contentType: newImage.ContentType
                            );

                            eventToUpdate.EventImgs.Add(new EventImg { ImgUrl = imagenUrl });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<ResponseEventDTO>(eventToUpdate);
        }
    }
}
