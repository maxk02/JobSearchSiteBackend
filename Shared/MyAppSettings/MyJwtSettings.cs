using System.ComponentModel.DataAnnotations;

namespace Shared.MyAppSettings;

public class MyJwtSettings
{
    [Required] public required string Issuer { get; init; }
    [Required] public required string Audience { get; init; }
    [Required] public required string SecretKey { get; init; }
}