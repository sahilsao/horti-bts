using HortiBts.Api.Data;
using HortiBts.Api.Repositories.Auth;
using HortiBts.Api.Repositories.Benefits;
using HortiBts.Api.Repositories.Components;
using HortiBts.Api.Repositories.Dashboard.Admin;
using HortiBts.Api.Repositories.Districts;
using HortiBts.Api.Repositories.Farmers;
using HortiBts.Api.Repositories.FinancialYears;
using HortiBts.Api.Repositories.Girdawari;
using HortiBts.Api.Repositories.MIDHComponents;
using HortiBts.Api.Repositories.MIDHSchemes;
using HortiBts.Api.Repositories.Notices;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Api.Repositories.SubDistricts;
using HortiBts.Api.Repositories.Target;
using HortiBts.Api.Repositories.Units;
using HortiBts.Api.Repositories.Villages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// ── Database Factory
builder.Services.AddScoped<IDbConnectionFactory, MySqlConnectionFactory>();

// ── Services
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ISchemeRepository, SchemeRepository>();
builder.Services.AddScoped<ISchemeDocRepository, SchemeDocRepository>();
builder.Services.AddScoped<IDistrictsRepository, DistrictsRepository>();
builder.Services.AddScoped<ISubDistrictsRepository, SubDistrictsRepository>();
builder.Services.AddScoped<IVillagesRepository, VillagesRepository>();
builder.Services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
builder.Services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IFinancialYearsRepository, FinancialYearsRepository>();
builder.Services.AddScoped<IFarmersDetailsRepository, FarmersDetailsRepository>();
builder.Services.AddHttpClient<ICropDetailRepository, CropDetailRepository>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
}); //girdawari details
builder.Services.AddScoped<IFarmersVerificationRepository, FarmersVerificationRepository>();
builder.Services.AddScoped<IBenefitsRepository, BenefitsRepository>();
builder.Services.AddScoped<INoticeRepository, NoticeRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<ITargetRepository, TargetRepository>();
builder.Services.AddScoped<IMIDHSchemeRepository, MIDHSchemeRepository>();
builder.Services.AddScoped<IMIDHSchemeDocRepository, MIDHSchemeDocRepository>();
builder.Services.AddScoped<IMIDHComponentTypeRepository, MIDHComponentTypeRepository>();
// CORS: required because the Blazor client is a STANDALONE app (separate origin),
// not hosted by this server project. Update the origin list for your actual client URLs.
const string BtsClientCorsPolicy = "HortiBtsClient";
const string PublicClientCorsPolicy = "HortiPublicClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(BtsClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5032",
                "https://localhost:7126",                // local Blazor dev server -- confirm your actual port
                "https://cghorticulture.gov.in",
                "https://www.cghorticulture.gov.in")     // production client origin, once deployed
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy(PublicClientCorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7188",               // local Blazor dev server -- confirm your actual port
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

app.UseHttpsRedirection();

app.UseCors(BtsClientCorsPolicy);
app.UseCors(PublicClientCorsPolicy);

app.UseAuthentication(); // will start doing something once JWT is added
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.Run();
