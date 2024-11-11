using Domain.Entities.Companies;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Users;

public class UserCompanyBookmark : BaseEntity
{
    public static UserCompanyBookmarkValidator Validator { get; } = new();

    public static Result<UserCompanyBookmark> Create(long userId, long companyId)
    {
        var bookmark = new UserCompanyBookmark(userId, companyId);

        var validationResult = Validator.Validate(bookmark);

        return validationResult.IsValid
            ? Result.Success(bookmark)
            : Result.Failure<UserCompanyBookmark>(validationResult.Errors);
    }
    private UserCompanyBookmark(long userId, long companyId)
    {
        UserId = userId;
        CompanyId = companyId;
    }
    
    public long UserId { get; private set; }
    public long CompanyId { get; private set; }
    
    
    public virtual User? User { get; set; }
    public virtual Company? Company { get; set; }
}