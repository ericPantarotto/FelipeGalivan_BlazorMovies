using Microsoft.JSInterop;

namespace BlazorMovies.Components
{
    public static class ExampleJsInteropNetCore3
    {
        public static ValueTask<string> Prompt(this IJSRuntime jsRuntime, string message)
        {
            return jsRuntime.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);
        }
    }
}
