using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMovies.Client.Pages {
    public partial class Counter
    {
        [Inject] IJSRuntime? JS { get; set; }

        private int currentCount = 0;
        IJSObjectReference? module;

        [JSInvokable]
        public async void IncrementCount()
        {
            var array = new double[] { 1, 2, 3, 4, 5 };
            double maxItem = array.Maximum();
            double minItem = array.Minimum();

            module = await JS!.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            await module!.InvokeVoidAsync("displayAlert", $"Max is {maxItem} and min is {minItem}");


            currentCount++;
        }
    }
}
