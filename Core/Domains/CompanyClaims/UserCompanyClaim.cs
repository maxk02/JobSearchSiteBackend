using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Companies;
using Core.Domains.UserProfiles;

namespace Core.Domains.CompanyClaims;

public class UserCompanyClaim : IEntityWithId
{
    public UserCompanyClaim(long userId, long companyId, long permissionId)
    {
        UserId = userId;
        CompanyId = companyId;
        PermissionId = permissionId;
    }

    public long Id { get; set; }
    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    public long PermissionId { get; private set; }

    public virtual UserProfile? User { get; set; }
    public virtual Company? Company { get; set; }
    public virtual CompanyClaim? CompanyPermission { get; set; }
}