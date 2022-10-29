using System.Net.Http.Headers;
using Broker.Infrastructure;
using Broker.Infrastructure.Integration.Services.Services.ERA;
using Broker.Services.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddServices(builder.Configuration);

// Add http services
builder.Services.AddHttpClient(nameof(ExchangeRatesApiServiceBase), httpClient =>
{
    var apiUrl = builder.Configuration.GetSection("Integrations:ExchangeRatesApi")["ApiUrl"] ?? string.Empty;
    var apiKey = builder.Configuration.GetSection("Integrations:ExchangeRatesApi")["ApiKey"] ?? string.Empty;

    httpClient.BaseAddress = new Uri(apiUrl);

    httpClient.DefaultRequestHeaders.Add(ApiKeyName, apiKey);
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
});

// Add controllers
builder.Services.AddControllers();

// Add database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(AppDbContext))));

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(AppCorsPolicyName,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(AppCorsPolicyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    private const string ApiKeyName = "apikey";
    private const string ApplicationJson = "application/json";
    private const string AppCorsPolicyName = "AppCorsPolicy";
}