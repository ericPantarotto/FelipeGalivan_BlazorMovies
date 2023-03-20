using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorMovies.Client;
using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<HttpClientWithToken>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<HttpClientWithoutToken>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

ConfigureServices(builder.Services);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IRepository, RepositoryInMemory>();
    services.AddScoped<IHttpService, HttpService>();
    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IMoviesRepository, MoviesRepository>();
    services.AddScoped<IAccountsRepository, AccountsRepository>();
    services.AddScoped<IRatingRepository, RatingRepository>();
    services.AddScoped<IDisplayMessage, DisplayMessage>();
    services.AddScoped<IUsersRepository, UserRepository>();

    services.AddApiAuthorization();
}
