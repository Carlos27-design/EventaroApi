using AutoMapper;
using EventaroApi.DTOs.UserDTOs;
using EventaroApi.Entities;

namespace EventaroApi.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
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
        }
    }
}
