using BlazorMovies.Server;
using BlazorMovies.Server.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<IdentityUser, ApplicationDbContext>()
    .AddProfileService<IdentityProfileService>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();


//builder.Services.AddScoped<IFileStorageService, AzureStorageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileStorageService, InAppStorageService>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(profileAssemblyMarkerTypes: typeof(Program));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
