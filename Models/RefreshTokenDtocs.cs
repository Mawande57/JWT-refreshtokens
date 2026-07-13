using Microsoft.Extensions.Configuration.UserSecrets;

namespace JWTtesting.Models
{
    public class RefreshTokenDtocs
    {
        public  Guid UserID { get; set; } 
        public string refreshtoken { get; set; } = string.Empty;
        
        
   }
}
