using ITS.Api.Middlewares;
using ITS.Application.Interfaces;
using ITS.Application.Services;
using ITS.Domain.HelperInterface;
using ITS.Domain.Repositories;
using ITS.Infrastructure.Helper;
using ITS.Infrastructure.Persistence;
using ITS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

// Register ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySQL(connectionString)
    );


// Add application services
builder.Services.AddScoped<IInmateService, InmateService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IOfficerService, OfficerService>();
builder.Services.AddScoped<ITransferService, TransferService>();

// Add repository implementations
builder.Services.AddScoped<IInmateRepository, InmateRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IOfficerRepository, OfficerRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<IDataAccessHelper, DataAccessHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/api", () => $"ITS Api started successfully on {DateTime.Now.ToString()}");

app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
