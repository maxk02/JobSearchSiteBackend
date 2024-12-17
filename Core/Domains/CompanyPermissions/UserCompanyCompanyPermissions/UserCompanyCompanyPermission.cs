using Core.Domains._Shared.Entities;
using Core.Domains.Companies;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.CompanyPermissions.UserCompanyCompanyPermissions;

public class UserCompanyCompanyPermission : BaseEntity
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

    public UserCompanyCompanyPermission(long userId, long companyId, long permissionId)
    {
        UserId = userId;
        CompanyId = companyId;
        PermissionId = permissionId;
    }

    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    public long PermissionId { get; private set; }

    public virtual UserProfile? User { get; set; }
    public virtual Company? Company { get; set; }
    public virtual CompanyPermission? CompanyPermission { get; set; }
}