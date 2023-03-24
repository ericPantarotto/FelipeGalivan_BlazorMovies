using BlazorMovies.Shared.DTOs;

namespace BlazorMovies.Shared.Repositories
{
    public interface IAccountsRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);
        Task<UserToken?> RenewToken();
    }
}
