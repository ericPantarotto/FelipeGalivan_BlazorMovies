using BlazorMovies.Components;

namespace BlazorMovies.Client.Helpers
{
    public class ExampleImplementation : IExampleInterface
    {
        public string GetValue()
        {
            return "From WebAssembly";
        }
    }
}
