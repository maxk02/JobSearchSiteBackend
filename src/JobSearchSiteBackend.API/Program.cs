using System.Reflection;
using System.Text.Json.Serialization;
using DotNetEnv;
using Hangfire;
using JobSearchSiteBackend.API.Middleware;
using JobSearchSiteBackend.API.Services;
using JobSearchSiteBackend.Core;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.Locations.Search;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Cookies;
using JobSearchSiteBackend.Core.Services.Search;
using JobSearchSiteBackend.Infrastructure;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs;
using JobSearchSiteBackend.Infrastructure.Persistence.EfCore;
using JobSearchSiteBackend.Shared.MyAppSettings;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);


//JobSearchSiteBackend.API general

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Env.Load("./vars.env");

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddOptions<MyAppSettings>()
    .Bind(builder.Configuration.GetSection(nameof(MyAppSettings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<MySmtpSettings>()
    .Bind(builder.Configuration.GetSection(nameof(MySmtpSettings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// builder.Services.AddAntiforgery(options =>
// {
//     options.HeaderName = "X-CSRF-TOKEN";
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:3000", "https://127.0.0.1:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    
    options.AddPolicy("AllowNextJsHttp", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true) // allow any origin dynamically
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


// JobSearchSiteBackend.API services
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ICurrentAccountService, JwtCurrentAccountService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Infrastructure services
builder.Services.ConfigurePersistenceWithIdentity(builder.Configuration);
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureJwtTokenGeneration();
builder.Services.ConfigureMemoryCache(builder.Configuration);
builder.Services.ConfigureBackgroundJobScheduler(builder.Configuration);
builder.Services.ConfigureFileStorage(builder.Configuration);
builder.Services.ConfigureEmailSender();
builder.Services.ConfigureSearch(builder.Configuration);
builder.Services.ConfigureTextExtraction();

// Core services
builder.Services.ConfigureCoreAutoMapper();
builder.Services.ConfigureUseCases();
builder.Services.ConfigureEmailRenderers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
});

var app = builder.Build();

// Getting assembly by some type
var infrastructureAssembly = typeof(JobSearchSiteBackend.Infrastructure.ServiceExtensions).Assembly;

// Implemented search repo types
List<Type> searchRepositories = [
    typeof(IJobSearchRepository),
    typeof(ILocationSearchRepository),
    typeof(IPersonalFileSearchRepository),
];

// Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // DB
    var dbContextSeeder = services.GetRequiredService<MainDataContextSeeder>();
    await dbContextSeeder.SeedAllAsync();
    
    // ElasticSearch
    foreach (var repoType in searchRepositories)
    {
        var repository = services.GetRequiredService(repoType);
                
        await ((dynamic)repository).SeedAsync();
    }
}

// Registering recurring jobs
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var registerer = services.GetRequiredService<IRecurringJobRegisterer>();
    await registerer.RegisterJobsAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Log the error to the console immediately
        Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
        Console.WriteLine(ex.StackTrace);

        // Re-throw so the standard error page can still show it (optional)
        throw; 
    }
});

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowNextJs");
app.UseCors("AllowNextJsHttp");

// app.UseMiddleware<CsrfProtectionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequireAuthWithEmailConfirmedMiddleware>();
app.UseMiddleware<CheckUserTokenMiddleware>();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");

app.Run();