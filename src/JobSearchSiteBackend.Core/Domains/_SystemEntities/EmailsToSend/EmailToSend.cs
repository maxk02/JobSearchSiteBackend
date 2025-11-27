using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains._SystemEntities.EmailsToSend;

public class EmailToSend : IEntityWithId, IEntityWithGuid
{
    public EmailToSend(long id, Guid guidIdentifier, string from, string to, string cc,
        string bcc, string subject, string body, bool isHtml)
    {
        Id = id;
        GuidIdentifier = guidIdentifier;
        From = from;
        To = to;
        Cc = cc;
        Bcc = bcc;
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
    }

    public long Id { get; private set; }
    public Guid GuidIdentifier { get; private set; }

    public string From { get; set; }
    public string To { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }

    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}