using HortiBts.Client;
using HortiBts.Client.MultiLanguage;
using HortiBts.Client.Services.Auth;
using HortiBts.Client.Services.Benefits;
using HortiBts.Client.Services.Common;
using HortiBts.Client.Services.Components;
using HortiBts.Client.Services.Dashboard;
using HortiBts.Client.Services.Districts;
using HortiBts.Client.Services.Farmers;
using HortiBts.Client.Services.FinancialYears;
using HortiBts.Client.Services.Girdawari;
using HortiBts.Client.Services.MIDHComponents;
using HortiBts.Client.Services.MIDHSchemes;
using HortiBts.Client.Services.Notices;
using HortiBts.Client.Services.Officers;
using HortiBts.Client.Services.Reports;
using HortiBts.Client.Services.Reports.Component;
using HortiBts.Client.Services.Reports.CurrentFY.Applications;
using HortiBts.Client.Services.Reports.HPMIS;
using HortiBts.Client.Services.Reports.PreviousFY.Applications;
using HortiBts.Client.Services.Reports.PreviousFY.BacklogYearly;
using HortiBts.Client.Services.Reports.PreviousFY.Comparative;
using HortiBts.Client.Services.Reports.PreviousFY.Yearly;
using HortiBts.Client.Services.Reports.Scheme;
using HortiBts.Client.Services.Schemes;
using HortiBts.Client.Services.SubDistricts;
using HortiBts.Client.Services.Target;
using HortiBts.Client.Services.Units;
using HortiBts.Client.Services.Users;
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
    //client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5170");
})
.AddHttpMessageHandler<RefreshTokenDelegatingHandler>()
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

builder.Services.AddSingleton<LanguageService>();

// ── Feature Services (Master Data) — API-backed implementations

// Authentication and Authorization Related
builder.Services.AddScoped<LoginHistoryApiService>();

// Common Related


builder.Services.AddScoped<DistrictsApiService>();
builder.Services.AddScoped<SubDistrictsApiService>();
builder.Services.AddScoped<VillagesApiService>();
builder.Services.AddScoped<FinancialYearsApiService>();
builder.Services.AddScoped<OfficersApiService>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<UsersApiService>();

// Dashboard Related
builder.Services.AddScoped<DashboardApiService>();

// Master Entry Related
builder.Services.AddScoped<NoticesApiService>();
builder.Services.AddScoped<SchemesApiService>();
builder.Services.AddScoped<GirdawariApiService>();
builder.Services.AddScoped<FarmersDetailsApiService>();
builder.Services.AddScoped<FarmersVerificationForUFPApiService>();
builder.Services.AddScoped<BenefitsApiService>();
builder.Services.AddScoped<MidhSchemesApiService>();
builder.Services.AddScoped<ComponentsApiService>();
builder.Services.AddScoped<UnitsApiService>();
builder.Services.AddScoped<TargetApiService>();
builder.Services.AddScoped<MIDHComponentTypeApiService>();
builder.Services.AddScoped<MIDHComponentApiService>();
builder.Services.AddScoped<MIDHSubComponentApiService>();

// Reports Related
builder.Services.AddScoped<YearlyFarmerRegistrationApiService>();
builder.Services.AddScoped<BacklogYearlyFarmerRegistrationApiService>();
builder.Services.AddScoped<ComparativeFarmerRegistrationApiService>();
builder.Services.AddScoped<FarmerBeneficiaryStatusApiService>();
builder.Services.AddScoped<FarmerApplicationsListApiService>();
builder.Services.AddScoped<NewFarmerApplicationsListApiService>();
// HPMIS
builder.Services.AddScoped<FarmersListHPMISApiService>();
//Scheme & Component
builder.Services.AddScoped<BacklogYearlySchemeWiseRegistrationApiService>();
builder.Services.AddScoped<BacklogYearlyComponentWiseRegistrationApiService>();

await builder.Build().RunAsync();
