using Azure.Core;
using JWTtesting.Data;
using JWTtesting.Entities;
using JWTtesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace JWTtesting.Services
{
    public class AuthService(AppDBContext _context, IConfiguration configuration) : IAuthService
    {
        public async Task<ResponseTokenDto?> LoginAsync(LoginUserDto loginUser)
        {
            var user_ = await _context.Users.FirstOrDefaultAsync(u => u.username == loginUser.username);
            if (user_ == null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user_, user_.passwordhash, loginUser.password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            
            ResponseTokenDto tokens = new ResponseTokenDto
            {
                accesstoken = generatetoken(),
                refreshtoken =GenerateToken(user_)
            };

            user_.RefreshtokenExpirydate = DateTime.UtcNow;
            user_.Refreshtoken = tokens.refreshtoken;
            await _context.SaveChangesAsync();
            return tokens;
        }

        public  async Task<User?> RegisterAsync(RegisterUserDto user)
        {
            var existing_user = await _context.Users.AnyAsync(u => u.username == user.username);
            if(existing_user)
            {
                return null;
            }

            var new_user = new User();
            var hashedPassword = new PasswordHasher<User>()
             .HashPassword(new_user, user.password);

            new_user.username = user.username;
            new_user.passwordhash = hashedPassword;
            new_user.role = user.role;
            
            _context.Users.Add(new_user);
            await _context.SaveChangesAsync();


            return new_user;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Name,user.username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.role)

            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
               issuer: configuration.GetValue<string>("AppSettings:Issuer"),
               audience: configuration.GetValue<string>("AppSettings:Audience"),
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(2),
               signingCredentials : creds
                );
            return new  JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async  Task<List<User>?> getUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users is null)
            {
                return null;
            }

            else return users;
        }

        public async Task<ResponseTokenDto?> RefreshTokenAsnc(RefreshTokenDtocs refreshTokenDtocs)
        {
            var user = await _context.Users.FindAsync(refreshTokenDtocs.UserID);

            if(user == null)
            {
                return null;
            }
            if(user.Refreshtoken != refreshTokenDtocs.refreshtoken || user.RefreshtokenExpirydate <= DateTime.UtcNow)
            {
                return null;
            }
            user.RefreshtokenExpirydate = DateTime.UtcNow.AddDays(6);
            user.Refreshtoken = refreshTokenDtocs.refreshtoken;


            await _context.SaveChangesAsync(); 


            ResponseTokenDto usertokens = new ResponseTokenDto
            { 
               accesstoken = GenerateToken(user),
               refreshtoken = generatetoken()
            };

            return usertokens;


        }

        public string generatetoken()
        {
            var num = new byte[32];
            using var rng =  RandomNumberGenerator.Create();
            rng.GetBytes(num);

            return Convert.ToBase64String(num);


        }

       
    }
}
