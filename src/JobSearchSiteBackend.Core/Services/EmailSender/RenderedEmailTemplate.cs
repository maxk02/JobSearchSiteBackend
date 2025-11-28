namespace JobSearchSiteBackend.Core.Services.EmailSender;

public class RenderedEmailTemplate : IEmailTemplate
{
    public RenderedEmailTemplate(string subject, string content, bool isHtml)
    {
        Subject = subject;
        Content = content;
        IsHtml = isHtml;
    }
    
    public string Subject { get; }
    public string Content { get; }
    public bool IsHtml { get; }
}