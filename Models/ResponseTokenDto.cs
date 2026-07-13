namespace JWTtesting.Models
{
    public class ResponseTokenDto
    {
        public string accesstoken { get; set; } = string.Empty;
        public string refreshtoken { get; set; } = string.Empty;
    }
}
