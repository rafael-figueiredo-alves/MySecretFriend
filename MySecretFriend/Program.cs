using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySecretFriend.Components;
using MySecretFriend.Context;
using MySecretFriend.Endpoints;
using MySecretFriend.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o EncryptionService
var encryptionConfig = builder.Configuration.GetSection("Encryption");

builder.Services.AddSingleton(new EncryptionService(
    encryptionConfig["Key"]!,
    encryptionConfig["IV"]!
));

builder.Services.AddDbContext<FriendsDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<FriendService>();
builder.Services.AddHttpClient<FriendService>((serviceProvider, client) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var request = httpContextAccessor.HttpContext?.Request;

    if (request != null)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}";
        client.BaseAddress = new Uri(baseUrl);
    }
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

app.MapFriendEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
