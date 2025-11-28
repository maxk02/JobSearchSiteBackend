using System.ComponentModel.DataAnnotations;

namespace JobSearchSiteBackend.Shared.MyAppSettings.Email;

public class MyDefaultEmailSenderSettings : IMyEmailSenderSettings
{
    [Required] public required string Name { get; init; }
    [Required] public required string EmailAddress { get; init; }
}