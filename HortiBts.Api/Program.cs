using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Repositories.ApplicationVerification.DDH;
using HortiBts.Api.Repositories.Auth;
using HortiBts.Api.Repositories.Benefits;
using HortiBts.Api.Repositories.Components;
using HortiBts.Api.Repositories.Dashboard.Admin;
using HortiBts.Api.Repositories.Dashboard.District;
using HortiBts.Api.Repositories.Districts;
using HortiBts.Api.Repositories.Farmers;
using HortiBts.Api.Repositories.FinancialYears;
using HortiBts.Api.Repositories.HPMIS;
using HortiBts.Api.Repositories.MIDHComponents;
using HortiBts.Api.Repositories.MIDHSchemes;
using HortiBts.Api.Repositories.Notices;
using HortiBts.Api.Repositories.Officers;
using HortiBts.Api.Repositories.Reports.Component;
using HortiBts.Api.Repositories.Reports.CurrentFY.Applications;
using HortiBts.Api.Repositories.Reports.HPMIS;
using HortiBts.Api.Repositories.Reports.PreviousFY.Applications;
using HortiBts.Api.Repositories.Reports.PreviousFY.BacklogYearly;
using HortiBts.Api.Repositories.Reports.PreviousFY.BeneficiaryStatus;
using HortiBts.Api.Repositories.Reports.PreviousFY.Comparative;
using HortiBts.Api.Repositories.Reports.PreviousFY.Yearly;
using HortiBts.Api.Repositories.Reports.Scheme;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Api.Repositories.SubDistricts;
using HortiBts.Api.Repositories.Target;
using HortiBts.Api.Repositories.Units;
using HortiBts.Api.Repositories.Users;
using HortiBts.Api.Repositories.Villages;
using HortiBts.Api.Services;
using HortiBts.Api.Services.Soil;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Dapper configuration
DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// ── Database Factory
builder.Services.AddScoped<IDbConnectionFactory, MySqlConnectionFactory>();

// ── Services
builder.Services.AddHttpClient<ISoilDetailsService, SoilDetailsService>();

// Authentication and Authorization Related
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
builder.Services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();

// Common Related
builder.Services.AddScoped<IDistrictsRepository, DistrictsRepository>();
builder.Services.AddScoped<ISubDistrictsRepository, SubDistrictsRepository>();
builder.Services.AddScoped<IVillagesRepository, VillagesRepository>();
builder.Services.AddScoped<IFinancialYearsRepository, FinancialYearsRepository>();
builder.Services.AddScoped<IOfficersRepository, OfficersRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// Dashboard Related
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
builder.Services.AddScoped<IDistrictDashboardRepository, DistrictDashboardRepository>();

// Master Entry Related
builder.Services.AddScoped<ISchemeRepository, SchemeRepository>();
builder.Services.AddScoped<ISchemeDocRepository, SchemeDocRepository>();
builder.Services.AddScoped<IFarmersDetailsByUFIDRepository, FarmersDetailsByUFIDRepository>();
builder.Services.AddScoped<IFarmersVerificationRepository, FarmersVerificationRepository>();
builder.Services.AddScoped<IBenefitsRepository, BenefitsRepository>();
builder.Services.AddScoped<INoticeRepository, NoticeRepository>();
builder.Services.AddScoped<INoticeDocRepository, NoticeDocRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<ITargetRepository, TargetRepository>();
builder.Services.AddScoped<IMIDHSchemeRepository, MIDHSchemeRepository>();
builder.Services.AddScoped<IMIDHSchemeDocRepository, MIDHSchemeDocRepository>();
builder.Services.AddScoped<IMIDHComponentTypeRepository, MIDHComponentTypeRepository>();
builder.Services.AddScoped<IMIDHComponentRepository, MIDHComponentRepository>();
builder.Services.AddScoped<IMIDHSubComponentRepository, MIDHSubComponentRepository>();

// Reports Related
builder.Services.AddScoped<IYearlyFarmerRegistrationRepository, YearlyFarmerRegistrationRepository>();
builder.Services.AddScoped<IBacklogFarmerRegistrationRepository, BacklogYearlyFarmerRegistrationRepository>();
builder.Services.AddScoped<IComparativeFarmerRegistrationRepository, ComparativeFarmerRegistrationRepository>();
builder.Services.AddScoped<IFarmerBeneficiaryStatusRepository, FarmerBeneficiaryStatusRepository>();
builder.Services.AddScoped<IFarmerApplicationsRepository, FarmerApplicationsRepository>();
builder.Services.AddScoped<INewFarmerApplicationsRepository, NewFarmerApplicationsRepository>();
// HPMIS report
builder.Services.AddScoped<IFarmersListHPMISRepository, FarmersListHPMISRepository>();
builder.Services.AddScoped<IFarmersDetailsHPMISRepository, FarmersDetailsHPMISRepository>();
// Scheme & Component
builder.Services.AddScoped<IBacklogYearlySchemeWiseRegistrationRepository, BacklogYearlySchemeWiseRegistrationRepository>();
builder.Services.AddScoped<IBacklogYearlyComponentWiseRegistrationRepository, BacklogYearlyComponentWiseRegistrationRepository>();

// Application Verification Related
builder.Services.AddScoped<ISchemeTypeWiseNewFarmerVerificationRepository, SchemeTypeWiseNewFarmerVerificationRepository>();
builder.Services.AddScoped<ISchemeWiseNewBeneficiaryApplicationRepository, SchemeWiseNewBeneficiaryApplicationRepository>();

builder.Services.AddScoped<ISchemeTypeWiseOldFarmerVerificationRepository, SchemeTypeWiseOldFarmerVerificationRepository>();
builder.Services.AddScoped<ISchemeWiseOldBeneficiaryApplicationRepository, SchemeWiseOldBeneficiaryApplicationRepository>();

builder.Services.AddScoped<IFarmersDetailsByApplicationIdRepository, FarmersDetailsByApplicationIdRepository>();

// CORS: required because the Blazor client is a STANDALONE app (separate origin),
// not hosted by this server project. Update the origin list for your actual client URLs.
const string BtsClientCorsPolicy = "HortiBtsClient";
const string PublicClientCorsPolicy = "HortiPublicClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(BtsClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5032",               // local Blazor dev server -- confirm your actual port
                "https://cghorticulture.gov.in",
                "https://www.cghorticulture.gov.in")     // production client origin, once deployed
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy(PublicClientCorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5066",               // local Blazor dev server -- confirm your actual port
                "https://cghorticulture.gov.in",
                "https://www.cghorticulture.gov.in")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // default route: /scalar/v1
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors(BtsClientCorsPolicy);
app.UseCors(PublicClientCorsPolicy);

app.UseAuthentication(); // will start doing something once JWT is added
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.Run();
