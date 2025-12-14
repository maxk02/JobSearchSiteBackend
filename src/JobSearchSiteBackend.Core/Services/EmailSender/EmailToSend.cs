using JobSearchSiteBackend.Shared.MyAppSettings.Email;

namespace JobSearchSiteBackend.Core.Services.EmailSender;

public class EmailToSend
{
    public EmailToSend(Guid guidIdentifier, string senderName, string senderEmail, string to, string? cc,
        string? bcc, string? subject, string? body, bool isHtml)
    {
        GuidIdentifier = guidIdentifier;
        SenderName = senderName;
        SenderEmail = senderEmail;
        To = to;
        Cc = cc;
        Bcc = bcc;
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
    }

    // public long Id { get; private set; }
    public Guid GuidIdentifier { get; private set; }

    public string SenderName { get; private set; }
    public string SenderEmail { get; private set; }
    public string To { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }

    public string? Subject { get; set; }
    public string? Body { get; set; }
    public bool IsHtml { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}