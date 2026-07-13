using JWTtesting.Entities;
using JWTtesting.Models;

namespace JWTtesting.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterUserDto user);
        Task<ResponseTokenDto?> LoginAsync(LoginUserDto loginUser);
        Task<List<User>?> getUsers();

        Task<ResponseTokenDto?> RefreshTokenAsnc(RefreshTokenDtocs refreshTokenDtocs);
    }
}
