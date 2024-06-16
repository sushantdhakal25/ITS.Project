using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using ITS.Api.Middlewares;
using ITS.Application.DTOs;
using ITS.Application.IdentityServer;
using ITS.Application.Interfaces;
using ITS.Application.Services;
using ITS.Domain.HelperInterface;
using ITS.Domain.Repositories;
using ITS.Infrastructure.Helper;
using ITS.Infrastructure.Persistence;
using ITS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

var origins = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors",
             b => b.WithOrigins(origins.ToArray())
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());
});

// Register ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySQL(connectionString)
    );


builder.Services.AddAutoMapper(typeof(OfficerDto).Assembly)
    .AddAutoMapper(typeof(MvLoginDto).Assembly);

builder.Services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = new PathString("/api/Officer/Login");
            })
             .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
             .AddDeveloperSigningCredential()
             .AddProfileService<ProfileService>()
             .AddDeveloperSigningCredential()
            .AddInMemoryApiResources(Configuration.GetApiResources())
            .AddInMemoryApiScopes(Configuration.GetApiScopes())
            .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
            .AddInMemoryClients(Configuration.GetClients());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration.GetSection("IdentityServerUrl").Value;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.OnStarting(async () =>
                            {
                                context.NoResult();
                                context.Response.ContentType = "text/plain";
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                await context.Response.WriteAsync(context.Exception.Message);
                            });
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            var payload = new JObject
                            {
                                ["error"] = context.Error,
                                ["error_description"] = context.ErrorDescription,
                                ["error_uri"] = context.ErrorUri
                            };
                            return context.Response.WriteAsync(payload.ToString());
                        }
                    };
                });


// Add application services
builder.Services.AddScoped<IInmateService, InmateService>()
.AddScoped<IFacilityService, FacilityService>()
.AddScoped<IOfficerService, OfficerService>()
.AddScoped<ITransferService, TransferService>()
.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
.AddScoped<IProfileService, ProfileService>()
.AddHttpContextAccessor();

// Add repository implementations
builder.Services.AddScoped<IInmateRepository, InmateRepository>()
.AddScoped<IFacilityRepository, FacilityRepository>()
.AddScoped<IOfficerRepository, OfficerRepository>()
.AddScoped<ITransferRepository, TransferRepository>()
.AddScoped<IDataAccessHelper, DataAccessHelper>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();

app.UseCors("AllowCors");

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapGet("/api", () => $"ITS Api started successfully on {DateTime.Now.ToString()}");

app.UseEndpoints(endpoints => endpoints
                             .MapControllers()
                             .RequireCors("AllowCors")
                             .RequireAuthorization()
);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
