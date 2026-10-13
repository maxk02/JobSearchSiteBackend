namespace JobSearchSiteBackend.Core.Services.EmailSender;

public interface IEmailTemplate
{
    public string Subject { get; }
    public string Content { get; }
}