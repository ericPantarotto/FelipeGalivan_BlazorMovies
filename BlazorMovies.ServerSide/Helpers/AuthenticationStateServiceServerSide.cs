using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorMovies.ServerSide.Helpers
{
    public class AuthenticationStateServiceServerSide : IAuthenticationStateService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationStateServiceServerSide(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetCurrentUserId()
        {
            var userState = await authenticationStateProvider.GetAuthenticationStateAsync();

            if (!userState.User.Identity!.IsAuthenticated)
            {
                return null!;
            }

            var claims = userState.User.Claims;

            var claimWithUserId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return claimWithUserId is null ? throw new ApplicationException("Could not find User's ID") : claimWithUserId.Value;
        }
    }
}
