namespace BlazorMovies.Shared.DTOs
{
    public class UserToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
