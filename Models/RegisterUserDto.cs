namespace JWTtesting.Models
{
    public class RegisterUserDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
