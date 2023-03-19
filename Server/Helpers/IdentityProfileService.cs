using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlazorMovies.Server.Helpers
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> claimsFactory;
        private readonly UserManager<IdentityUser> userManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            UserManager<IdentityUser> userManager)
        {
            this.claimsFactory = claimsFactory;
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(userId);
            var claimsPrincipal = await claimsFactory.CreateAsync(user!);
            var claims = claimsPrincipal.Claims.ToList();

            var claimsDB = await userManager.GetClaimsAsync(user!);

            //Note: we are doing this mapping as in the user controller we use ClaimType.Roles
            //if we had used Oidc from th begining we could use JwtClaimTypes.Role directly to avoid this mapping
            List<Claim> mappedClaims = new();
            foreach (var claim in claimsDB)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    mappedClaims.Add(new Claim(JwtClaimTypes.Role, claim.Value));
                }
                else
                {
                    mappedClaims.Add(claim);
                }
            }

            claims.AddRange(mappedClaims);
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(userId);
            context.IsActive = user is not null;
        }
    }
}
