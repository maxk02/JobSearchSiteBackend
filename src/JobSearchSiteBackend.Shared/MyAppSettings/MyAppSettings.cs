using System.ComponentModel.DataAnnotations;

namespace JobSearchSiteBackend.Shared.MyAppSettings;

public class MyAppSettings
{
    [Required] [MinLength(1)] public required string FrontendDomainName { get; init; }
}