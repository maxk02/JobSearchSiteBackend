using API.Middleware.Auth;
using API.Services.Auth;
using Core;
using Core.Domains.Accounts;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.Countries;
using Core.Domains.Cvs;
using Core.Domains.JobApplications;
using Core.Domains.JobContractTypes;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.Locations;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Core.Services.Auth;
using Infrastructure;
using Shared.MyAppSettings;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<MyAppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<MySmtpSettings>()
    .Bind(builder.Configuration.GetSection("SmtpSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 2048; // "MB"
});

builder.Services.AddScoped<ICurrentAccountService, JwtCurrentAccountService>();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureJwtGenerationService();
builder.Services.ConfigurePersistenceWithIdentity(builder.Configuration);

builder.Services.ConfigureHangfire();
builder.Services.ConfigureAmazonS3(builder.Configuration);
builder.Services.ConfigureMailKit();
builder.Services.ConfigureElasticSearch(builder.Configuration);
builder.Services.ConfigureTextExtraction();

builder.Services.ConfigureAccountUseCases();
builder.Services.ConfigureCompanyUseCases();
builder.Services.ConfigureCompanyClaimUseCases();
builder.Services.ConfigureCountryUseCases();
builder.Services.ConfigureCvUseCases();
builder.Services.ConfigureJobApplicationUseCases();
builder.Services.ConfigureJobContractTypeUseCases();
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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CheckUserTokenMiddleware>();

app.MapControllers();

app.Run();