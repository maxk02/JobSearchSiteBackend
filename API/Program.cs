using API.Middleware.Auth;
using Infrastructure;
using Shared.MyAppSettings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MyAppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 2048; // "MB"
});

builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.ConfigurePersistenceWithIdentity(builder.Configuration);
builder.Services.ConfigureAmazonS3(builder.Configuration);
builder.Services.ConfigureSendGrid(builder.Configuration);

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