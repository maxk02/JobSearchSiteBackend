using API.Domains._Shared.EntityInterfaces;
using API.Domains.Companies;
using API.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace API.Domains.CompanyPermissions.UserCompanyCompanyPermissions;

public class UserCompanyCompanyPermission : IEntityWithId
{
    public static UserCompanyCompanyPermissionValidator Validator { get; } = new();

    public static Result<UserCompanyCompanyPermission> Create(long userId, long companyId, long companyPermissionId)
    {
        var uccPermission = new UserCompanyCompanyPermission(userId, companyId, companyPermissionId);

        var validationResult = Validator.Validate(uccPermission);

        return validationResult.IsValid
            ? uccPermission
            : Result<UserCompanyCompanyPermission>.Invalid(validationResult.AsErrors());
    }

    private UserCompanyCompanyPermission(long userId, long companyId, long permissionId)
    {
        UserId = userId;
        CompanyId = companyId;
        PermissionId = permissionId;
    }
    
    public long Id { get; private set; }

    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    public long PermissionId { get; private set; }

    public virtual UserProfile? User { get; set; }
    public virtual Company? Company { get; set; }
    public virtual CompanyPermission? CompanyPermission { get; set; }
}