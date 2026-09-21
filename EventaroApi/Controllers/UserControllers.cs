using EventaroApi.DTOs.UserDTOs;
using EventaroApi.Entities;
using EventaroApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventaroApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserControllers: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserControllers(IUserService userService, UserManager<User> userManager)
        {
            this._userService = userService;
            this._userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserInfoDTO>> Register([FromBody] CreateUserDTO createUser) 
        {
            try
            {
                var result = await _userService.CreateUser(createUser);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] LoginDTO login) 
        {
            try
            {
                var result = await _userService.Login(login);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ProfileDTO>> GetProfile()
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid Token" });
                }

                var profile = await _userService.GetProfile(userId);
                return Ok(profile);
            }catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
            
        }
    }
}
