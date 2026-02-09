using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class CompanyEmployeeInvitation : IEntityWithId, IEntityWithGuid, IEntityWithDateTimeCreatedUtc
{
    public CompanyEmployeeInvitation(long companyId, long invitedUserId, long senderUserId)
    {
        CompanyId = companyId;
        InvitedUserId = invitedUserId;
        SenderUserId = senderUserId;
    }
    
    public long Id { get; set; }
    
    public Guid GuidIdentifier { get; set; } = Guid.NewGuid();
    
    public DateTime DateTimeCreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime DateTimeValidUtc { get; set; }  = DateTime.UtcNow.AddHours(1);
    
    public long CompanyId { get; set; }

    public long InvitedUserId { get; set; }

    public long SenderUserId { get; set; }

    public bool IsAccepted { get; set; }
    
    public Company? Company { get; set; }
    public UserProfile? InvitedUser { get; set; }
    public UserProfile? SenderUser { get; set; }
}