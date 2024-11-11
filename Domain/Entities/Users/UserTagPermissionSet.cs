using Domain.Entities.Companies;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Users;

public class UserTagPermissionSet : BaseEntity
{
    public static UserTagPermissionSetValidator Validator { get; } = new();
    
    public static Result<UserTagPermissionSet> Create(long userId, long tagId,
        bool canManageApplications, bool canCreateEditDeleteJobs,
        bool isTagAdmin, bool isTagOwner)
    {
        var permissionSet = new UserTagPermissionSet(userId, tagId,
            canManageApplications, canCreateEditDeleteJobs, isTagAdmin, isTagOwner);

        var validationResult = Validator.Validate(permissionSet);

        return validationResult.IsValid
            ? Result.Success(permissionSet)
            : Result.Failure<UserTagPermissionSet>(validationResult.Errors);
    }

    private UserTagPermissionSet(long userId, long tagId,
        bool canManageApplications, bool canCreateEditDeleteJobs,
        bool isTagAdmin, bool isTagOwner)
    {
        UserId = userId;
        TagId = tagId;
        CanManageApplications = canManageApplications;
        CanCreateEditDeleteJobs = canCreateEditDeleteJobs;
        IsTagAdmin = isTagAdmin;
        IsTagOwner = isTagOwner;
    }
    
    public long UserId { get; private set; }
    public long TagId { get; private set; }
    
    public bool CanManageApplications { get; private set; }
    public bool CanCreateEditDeleteJobs { get; private set; }
    public bool IsTagAdmin { get; private set; }
    public bool IsTagOwner { get; private set; }
    
    public virtual User? User { get; set; }
    public virtual Company? Tag { get; set; }
}