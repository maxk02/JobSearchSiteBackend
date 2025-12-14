using System.ComponentModel.DataAnnotations;

namespace JobSearchSiteBackend.Shared.MyAppSettings.Email;

public class MyDefaultEmailSenderOptions
{
    [Required] public required string Name { get; init; }
    [Required] public required string EmailAddress { get; init; }
}