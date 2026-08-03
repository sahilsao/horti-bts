using HortiBts.Api.Data;
using HortiBts.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnectionFactory, MySqlConnectionFactory>();
builder.Services.AddScoped<ISchemeRepository, SchemeRepository>();
builder.Services.AddScoped<ISchemeDocRepository, SchemeDocRepository>();

// CORS: required because the Blazor client is a STANDALONE app (separate origin),
// not hosted by this server project. Update the origin list for your actual client URLs.
const string ClientCorsPolicy = "HortiPublicClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5032",
                "https://localhost:7126",           // local Blazor dev server -- confirm your actual port
                "https://cghorticulture.gov.in",
                "https://www.cghorticulture.gov.in")     // production client origin, once deployed
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(ClientCorsPolicy);
app.MapControllers();

app.Run();
