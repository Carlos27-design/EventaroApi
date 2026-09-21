using EventaroApi.DTOs.UserDTOs;
using EventaroApi.Entities;

namespace EventaroApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ProfileDTO> GetProfile(string userId);
        public Task<UserInfoDTO> CreateUser(CreateUserDTO createUser);
        public Task<TokenResponseDTO> Login(LoginDTO login);
    }
}
