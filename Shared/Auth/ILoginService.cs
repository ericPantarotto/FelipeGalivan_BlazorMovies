using BlazorMovies.Shared.DTOs;

namespace BlazorMovies.Shared.Auth
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);
        Task Logout();
        Task TryRenewToken();
    }
}
