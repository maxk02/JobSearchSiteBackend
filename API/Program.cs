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
using Core.Services.Auth;
using Core.Services.Cookies;
using DotNetEnv;
using Infrastructure;
using Shared.MyAppSettings;

var builder = WebApplication.CreateBuilder(args);

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

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 2048; // "MB"
});

builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ICurrentAccountService, JwtCurrentAccountService>();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureJwtGenerationService();

builder.Services.ConfigureAutoMapper();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.ConfigurePersistenceWithIdentity(builder.Configuration);
builder.Services.ConfigureMemoryCache();
builder.Services.ConfigureBackgroundJobScheduler();
builder.Services.ConfigureFileStorage(builder.Configuration);
builder.Services.ConfigureEmailSender();
builder.Services.ConfigureSearch(builder.Configuration);
builder.Services.ConfigureTextExtraction();

builder.Services.ConfigureAccountUseCases();
builder.Services.ConfigureCompanyUseCases();
builder.Services.ConfigureCompanyClaimUseCases();
builder.Services.ConfigureCountryUseCases();
builder.Services.ConfigureJobApplicationUseCases();
builder.Services.ConfigureJobFolderClaimUseCases();
builder.Services.ConfigureJobFolderUseCases();
builder.Services.ConfigureJobUseCases();
builder.Services.ConfigureLocationUseCases();
builder.Services.ConfigurePersonalFileUseCases();
builder.Services.ConfigureUserProfileUseCases();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
});

var app = builder.Build();

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

app.UseMiddleware<CheckUserTokenMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();