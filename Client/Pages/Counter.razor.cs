using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorMovies.Client.Pages {
    public partial class Counter
    {
        [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
        private int currentCount = 0;

        public async Task IncrementCount()
        {
            AuthenticationState authState = await AuthenticationState!;
            var user = authState.User;

            if (user.Identity!.IsAuthenticated)
            {
                currentCount++;
            }
            else
            {
                currentCount--;
            }

        }
    }
}
