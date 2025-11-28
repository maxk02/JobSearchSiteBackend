namespace JobSearchSiteBackend.Shared.MyAppSettings.Email;

public interface IMyEmailSenderSettings
{
    public string Name { get; init; }
    public string EmailAddress { get; init; }
}