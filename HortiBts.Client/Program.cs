using HortiBts.Client;
using HortiBts.Client.MultiLanguage;
using HortiBts.Client.Services.Auth;
using HortiBts.Client.Services.Benefits;
using HortiBts.Client.Services.Common;
using HortiBts.Client.Services.Dashboard;
using HortiBts.Client.Services.Districts;
using HortiBts.Client.Services.Farmers;
using HortiBts.Client.Services.FinancialYears;
using HortiBts.Client.Services.Girdawari;
using HortiBts.Client.Services.Schemes;
using HortiBts.Client.Services.SubDistricts;
using HortiBts.Client.Services.Villages;
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
builder.Services.AddScoped<AuthApiService>();
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

builder.Services.AddSingleton<LanguageService>();

// ── Feature Services (Master Data) — API-backed implementations

builder.Services.AddScoped<DistrictsApiService>();
builder.Services.AddScoped<SubdistrictsApiService>();
builder.Services.AddScoped<VillagesApiService>();
builder.Services.AddScoped<LoginHistoryApiService>();
builder.Services.AddScoped<DashboardApiService>();
builder.Services.AddScoped<FinancialYearsApiService>();
builder.Services.AddScoped<GirdawariApiService>();
builder.Services.AddScoped<FarmersDetailsApiService>();
builder.Services.AddScoped<FarmersVerificationForUFPApiService>();
builder.Services.AddScoped<BenefitsApiService>();
builder.Services.AddScoped<SchemesApiService>();
await builder.Build().RunAsync();
