using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims;

public class UserCompanyClaim : IEntityWithId
{
    public UserCompanyClaim(long userId, long companyId, long claimId)
    {
        UserId = userId;
        CompanyId = companyId;
        ClaimId = claimId;
    }

    public long Id { get; set; }
    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    public long ClaimId { get; private set; }

    public UserProfile? User { get; set; }
    public Company? Company { get; set; }
    public CompanyClaim? CompanyClaim { get; set; }
}