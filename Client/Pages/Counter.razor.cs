using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [Inject] SingletonService? singleton { get; set; }
        [Inject] TransientService? transient { get; set; }
        [Inject] IJSRuntime? js { get; set; }
        //[CascadingParameter] public AppState AppState { get; set; }

        private int currentCount = 0;
        private static int currentCountStatic = 0;
        //IJSObjectReference module;


        [JSInvokable]
        public async Task IncrementCount()
        {
            //module = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            //await module.InvokeVoidAsync("displayAlert", "hello world");

            currentCount++;
            singleton!.Value += 1;
            transient!.Value += 1;
            currentCountStatic++;
            await js!.InvokeVoidAsync("dotnetStaticInvocation");
        }

        private async Task IncrementCountJavaScript()
        {
            await js!.InvokeVoidAsync("dotnetInstanceInvocation",
                DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public static Task<int> GetCurrentCount()
        {
            return Task.FromResult(currentCountStatic);
        }
    }
}
