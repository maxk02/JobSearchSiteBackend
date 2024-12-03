using Domain.Entities.Companies;
using Domain.Entities.CompanyPermissions;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Entities.UserCompanyCompanyPermissions;

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

    private UserCompanyCompanyPermission(long userId, long companyId, long permissionId)
    {
        UserId = userId;
        CompanyId = companyId;
        PermissionId = permissionId;
    }

    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    public long PermissionId { get; private set; }

    public virtual User? User { get; set; }
    public virtual Company? Company { get; set; }
    public virtual CompanyPermission? CompanyPermission { get; set; }
}