using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class CompanyEmployeeInvitation : IEntityWithId, IEntityWithGuid, IEntityWithDateTimeCreatedUtc
{
    public CompanyEmployeeInvitation(long companyId, string invitedUserEmail, long senderUserId)
    {
        CompanyId = companyId;
        InvitedUserEmail = invitedUserEmail;
        SenderUserId = senderUserId;
    }
    
    public long Id { get; set; }
    
    public Guid GuidIdentifier { get; set; } = Guid.NewGuid();
    
    public DateTime DateTimeCreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime DateTimeValidUtc { get; set; }  = DateTime.UtcNow.AddHours(1);
    
    public long CompanyId { get; set; }

    public string InvitedUserEmail { get; set; }

    public long SenderUserId { get; set; }

    public bool IsAccepted { get; set; }
    
    public Company? Company { get; set; }
    public UserProfile? SenderUser { get; set; }
}