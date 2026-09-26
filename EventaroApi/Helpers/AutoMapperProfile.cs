using AutoMapper;
using EventaroApi.DTOs.EventDTOs;
using EventaroApi.DTOs.EventTypeDTOs;
using EventaroApi.DTOs.OrganizationDTOs;
using EventaroApi.DTOs.UserDTOs;
using EventaroApi.Entities;

namespace EventaroApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Mapper of User
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(src => src.OrganizationId));
            CreateMap<User, UserInfoDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
            CreateMap<User, ProfileDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            //Mapper of Organization
            CreateMap<CreateOrganizationDTO, Organization>();
            CreateMap<Organization, ListOrganizationDTO>();
            CreateMap<Organization, ResponseOrganizationDTO>();

            //Mapper of EventType
            CreateMap<CreateEventTypeDTO, EventType>();
            CreateMap<EventType, ResponseEventTypeDTO>();
            CreateMap<EventType, ListEventTypeDTO>();

            //Mapper of Event
            CreateMap<CreateEventDTO, Event>()
                .ForMember(dest => dest.Ubication, opt => opt.Ignore())
                .ForMember(dest => dest.UbicationId, opt => opt.Ignore())
                .ForMember(dest => dest.EventImgs, opt => opt.Ignore());
            CreateMap<UpdateEventDTO, Event>()
                .ForMember(dest => dest.Ubication, opt => opt.Ignore())
                .ForMember(dest => dest.UbicationId, opt => opt.Ignore())
                .ForMember(dest => dest.EventImgs, opt => opt.Ignore());
            CreateMap<Event, ResponseEventDTO>()
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.Name : null))
                .ForMember(dest => dest.EventTypeName, opt => opt.MapFrom(src => src.EventType != null ? src.EventType.Name : null))
                .ForMember(dest => dest.Ubication, opt => opt.MapFrom(src => src.Ubication != null ? src.Ubication.Address : null))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.EventImgs.Select(img => img.ImgUrl).ToList()));
            CreateMap<Event, ResponseEventDTO>()
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.Name : null))
                .ForMember(dest => dest.EventTypeName, opt => opt.MapFrom(src => src.EventType != null ? src.EventType.Name : null))
                .ForMember(dest => dest.Ubication, opt => opt.MapFrom(src => src.Ubication != null ? src.Ubication.Address : null))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.EventImgs.Select(img => img.ImgUrl).ToList()));
        }
    }
}
