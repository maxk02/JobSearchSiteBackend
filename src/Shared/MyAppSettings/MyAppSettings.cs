using System.ComponentModel.DataAnnotations;

namespace Shared.MyAppSettings;

public class MyAppSettings
{
    [Required] [MinLength(1)] public required string DomainName { get; init; }
}