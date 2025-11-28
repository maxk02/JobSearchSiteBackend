using System.ComponentModel.DataAnnotations;

namespace JobSearchSiteBackend.Shared.MyAppSettings.Email;

public class MySmtpSettings
{
    [Required] public required string Server { get; init; }
    [Required] public required int Port { get; init; }
    [Required] public required string Username { get; init; }
    [Required] public required string Password { get; init; }
    [Required] public required bool EnableSsl { get; init; }
}