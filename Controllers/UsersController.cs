using JWTtesting.Entities;
using JWTtesting.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JWTtesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IAuthService authService) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> getusers()
        {
            var users = await authService.getUsers();

            return users == null || users.Count == 0 ? NoContent() : Ok(users);
        }
    }
}
