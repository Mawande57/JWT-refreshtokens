using JWTtesting.Entities;
using JWTtesting.Models;
using JWTtesting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JWTtesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("regiter")]
        public async Task<ActionResult<User>> register(RegisterUserDto user)
        {
            var user1 = await authService.RegisterAsync(user);
            if(user1 is null)
            {
                return BadRequest("User already exists");
            }
            return Created("api/authcontroller/register" , user1);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseTokenDto>> login(LoginUserDto loginUserDto)
        {
            var result = await authService.LoginAsync(loginUserDto);
            if(result == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(result);
        }
    
        [HttpPost("refreshtoken")]
        public async Task<ActionResult<ResponseTokenDto?>> refreshtoken(RefreshTokenDtocs refreshTokenDtocs )
        {
            var result = await authService.RefreshTokenAsnc(refreshTokenDtocs);
            if(result == null)
            {
                return BadRequest("Invalid user or accesstoken");
            }

            return Ok(result);
        }
    }

}
