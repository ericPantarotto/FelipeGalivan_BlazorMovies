using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorMovies.Client;
using BlazorMovies.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorMovies.Client.Auth;
using BlazorMovies.Components;
using BlazorMovies.Shared.Repositories;
using BlazorMovies.Components.Helpers;
using BlazorMovies.Client.Repository;
using BlazorMovies.Components.Auth;
using BlazorMovies.Shared.Auth;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

ConfigureServices(builder.Services);

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = await js.InvokeAsync<string>("getFromLocalStorage", "culture");

CultureInfo selectedCulture = new("en-US");

if (culture is not null)
{
    selectedCulture = new CultureInfo(culture);
}

CultureInfo.DefaultThreadCurrentCulture = selectedCulture;
CultureInfo.DefaultThreadCurrentUICulture = selectedCulture;

await host.RunAsync();
//await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services)
{
    services.AddLocalization();
    services.AddTransient<IRepository, RepositoryInMemory>();
    services.AddScoped<IHttpService, HttpService>();
    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IMoviesRepository, MoviesRepository>();
    services.AddScoped<IAccountsRepository, AccountsRepository>();
    services.AddScoped<IRatingRepository, RatingRepository>();
    services.AddScoped<IDisplayMessage, DisplayMessage>();
    services.AddScoped<IUsersRepository, UserRepository>();
    services.AddTransient<IExampleInterface, ExampleImplementation>();


    services.AddAuthorizationCore();

    services.AddScoped<JWTAuthenticationStateProvider>();
    services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(
        provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

    services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(
        provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

    services.AddScoped<TokenRenewer>();
}
