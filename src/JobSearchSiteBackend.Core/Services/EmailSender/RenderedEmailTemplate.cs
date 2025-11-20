namespace JobSearchSiteBackend.Core.Services.EmailSender;

public class RenderedEmailTemplate : IEmailTemplate
{
    public RenderedEmailTemplate(string subject, string content)
    {
        Subject = subject;
        Content = content;
    }
    
    public string Subject { get; }
    public string Content { get; }
}