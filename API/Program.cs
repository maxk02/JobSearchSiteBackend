using System.Reflection;
using System.Text.Json.Serialization;
using API.Middleware;
using API.Services;
using Core;
using Core.Domains.Accounts;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.Countries;
using Core.Domains.JobApplications;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.Locations;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Core.Persistence;
using Core.Services.Auth;
using Core.Services.Cookies;
using Core.Services.Search;
using DotNetEnv;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Shared.MyAppSettings;

var builder = WebApplication.CreateBuilder(args);


//API general

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Env.Load();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<MyAppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<MySmtpSettings>()
    .Bind(builder.Configuration.GetSection("SmtpSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 2048; // "MB"
});


// API services
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ICurrentAccountService, JwtCurrentAccountService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Infrastructure services
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureJwtTokenGeneration();
builder.Services.ConfigurePersistenceWithIdentity(builder.Configuration);
builder.Services.ConfigureMemoryCache();
builder.Services.ConfigureBackgroundJobScheduler();
builder.Services.ConfigureFileStorage(builder.Configuration);
builder.Services.ConfigureEmailSender();
builder.Services.ConfigureSearch(builder.Configuration);
builder.Services.ConfigureTextExtraction();

// Core services
builder.Services.ConfigureCoreAutoMapper();
builder.Services.ConfigureUseCases();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
});

var app = builder.Build();


// Implemented search repo types
var searchRepositories = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(type => 
        type.IsClass && 
        !type.IsAbstract && 
        type.GetInterfaces().Any(i => 
            i.IsGenericType && 
            i.GetGenericTypeDefinition() == typeof(ISearchRepository<>)))
    .ToList();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // DB
    var dbContext = services.GetRequiredService<MainDataContext>();
    await MainDataContextSeed.SeedAsync(dbContext);
    
    // ElasticSearch
    foreach (var repoType in searchRepositories)
    {
        try
        {
            var interfaceType = repoType.GetInterfaces()
                .First(i => i.IsGenericType && 
                            i.GetGenericTypeDefinition() == typeof(ISearchRepository<>));
            var genericArg = interfaceType.GetGenericArguments()[0];
            var genericInterfaceType = typeof(ISearchRepository<>).MakeGenericType(genericArg);
            var repository = services.GetRequiredService(genericInterfaceType);
                
            await ((dynamic)repository).SeedAsync();
            Console.WriteLine($"Seeded {repoType.Name} successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding {repoType.Name}: {ex.Message}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowNextJs");

// app.UseRouting();

app.UseMiddleware<CsrfProtectionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequireAuthWithEmailConfirmedMiddleware>();
app.UseMiddleware<CheckUserTokenMiddleware>();

app.MapControllers();

app.Run();