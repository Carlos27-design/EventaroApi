using AutoMapper;
using EventaroApi.Data;
using EventaroApi.DTOs.UserDTOs;
using EventaroApi.Entities;
using EventaroApi.Enums;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventaroApi.Services
{
    public class UserServices: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _rolesManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserServices(
            UserManager<User> _userManager,
            RoleManager<IdentityRole> rolesManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IMapper mapper
        )
        {
            this._userManager = _userManager;
            this._rolesManager = rolesManager;
            this._configuration = configuration;
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<UserInfoDTO> CreateUser(CreateUserDTO createUser)
        {
            var existingUser = await _userManager.FindByEmailAsync(createUser.Email);

            if(existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var newUser = _mapper.Map<User>(createUser);

            newUser.Status = StatusBase.Active;
            newUser.CreatedAt = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(newUser, createUser.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error user create: {errors}");
            }

            string roleToAssign = newUser.OrganizationId.HasValue ? "Organizer" : "User";

            if(!await _rolesManager.RoleExistsAsync(roleToAssign))
            {
                await _rolesManager.CreateAsync(new IdentityRole(roleToAssign));
            }

            await _userManager.AddToRoleAsync(newUser, roleToAssign);

            return _mapper.Map<UserInfoDTO>(newUser);
        }

        public async Task<ProfileDTO> GetProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";

            var profileDTO = _mapper.Map<ProfileDTO>(user);

            profileDTO.Role = userRole;

            return profileDTO;
        }

        public async Task<TokenResponseDTO> Login(LoginDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                throw new Exception("Credentials invalid");
            }

            if(await _userManager.IsLockedOutAsync(user))
            {
                throw new Exception("User is locked out");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(user);
                throw new Exception("Credencials invalid");
            }

            var userInfo = _mapper.Map<UserInfoDTO>(user);

            return await GenerateToken(userInfo);
        }



        //Generar un token JWT para el usuario autenticado
        private async Task<TokenResponseDTO> GenerateToken(UserInfoDTO userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.GivenName, userInfo.FirstName),
                new Claim(ClaimTypes.Surname, userInfo.LastName)
            };

            if (userInfo.OrganizationId.HasValue)
            {
                claims.Add(new Claim("OrganizationId", userInfo.OrganizationId.Value.ToString()));
            }

            var identityUser = await _userManager.FindByEmailAsync(userInfo.Email);

            if(identityUser == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);


            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return new TokenResponseDTO
            {
                Token = tokenString,
                Expiration = expiration
            };
        }
    }
}
