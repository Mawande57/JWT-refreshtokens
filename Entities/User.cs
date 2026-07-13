using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JWTtesting.Entities
{
    public class User
    {
        
        public Guid Id { get; set; }
        public string username { get; set; } = string.Empty;
        public string passwordhash { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

        public string accesstoken { get; set; } = string.Empty;
        public string Refreshtoken { get; set; } = string.Empty;

        public DateTime RefreshtokenExpirydate { get; set; }




    }
}
