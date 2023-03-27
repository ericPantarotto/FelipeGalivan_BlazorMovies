namespace BlazorMovies.SharedBackend.Helpers
{
    public interface IAuthenticationStateService
    {
        Task<string> GetCurrentUserId();
    }
}
