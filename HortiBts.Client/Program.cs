using HortiBts.Client;
using HortiBts.Client.MultiLanguage;
using HortiBts.Client.Services.Auth;
using HortiBts.Client.Services.Common;
using HortiBts.Client.Services.Districts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// ── Auth

builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<TokenAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddTransient<RefreshTokenDelegatingHandler>();

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<LocalStorageService>();

// HttpClient pointed at the Api project (not the WASM host itself)
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7202/");
})
.AddHttpMessageHandler<RefreshTokenDelegatingHandler>()
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

builder.Services.AddScoped<AuthApiService>();

// ── Feature Services (Master Data) — API-backed implementations

builder.Services.AddScoped<DistrictsApiService>();
builder.Services.AddScoped<LanguageService>();
await builder.Build().RunAsync();
