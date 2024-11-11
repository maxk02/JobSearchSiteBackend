using Domain.Entities.Companies;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Users;

public class UserCompanyPermissionSet : BaseEntity
{
    public static UserCompanyPermissionSetValidator Validator { get; } = new();

    public static Result<UserCompanyPermissionSet> Create(long userId, long companyId,
        bool canCreateTags, bool isCompanyAdmin, bool isCompanyOwner)
    {
        var permissionSet = new UserCompanyPermissionSet(userId, companyId,
            canCreateTags, isCompanyAdmin, isCompanyOwner);

        var validationResult = Validator.Validate(permissionSet);

        return validationResult.IsValid
            ? Result.Success(permissionSet)
            : Result.Failure<UserCompanyPermissionSet>(validationResult.Errors);
    }
    
    private UserCompanyPermissionSet(long userId, long companyId,
        bool canCreateTags, bool isCompanyAdmin, bool isCompanyOwner)
    {
        UserId = userId;
        CompanyId = companyId;
        CanCreateTags = canCreateTags;
        IsCompanyAdmin = isCompanyAdmin;
        IsCompanyOwner = isCompanyOwner;
    }
    
    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    
    public bool CanCreateTags { get; private set; }
    public bool IsCompanyAdmin { get; private set; }
    public bool IsCompanyOwner { get; private set; }
    
    public virtual User? User { get; set; }
    public virtual Company? Company { get; set; }
}