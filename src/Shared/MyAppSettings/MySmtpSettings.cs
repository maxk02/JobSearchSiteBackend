using System.ComponentModel.DataAnnotations;

namespace Shared.MyAppSettings;

public class MySmtpSettings
{
    [Required] public required string Server { get; init; }
    [Required] public required int Port { get; init; }
    [Required] public required string SenderName { get; init; }
    [Required] public required string SenderEmail { get; init; }
    [Required] public required string Username { get; init; }
    [Required] public required string Password { get; init; }
    [Required] public required bool EnableSsl { get; init; }
}