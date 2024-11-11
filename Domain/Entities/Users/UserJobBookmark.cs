using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Users;

public class UserJobBookmark : BaseEntity
{
    public static UserJobBookmarkValidator Validator { get; } = new();

    public static Result<UserJobBookmark> Create(long userId, long jobId)
    {
        var bookmark = new UserJobBookmark(userId, jobId);

        var validationResult = Validator.Validate(bookmark);

        return validationResult.IsValid
            ? Result.Success(bookmark)
            : Result.Failure<UserJobBookmark>(validationResult.Errors);
    }
    
    private UserJobBookmark(long userId, long jobId)
    {
        UserId = userId;
        JobId = jobId;
    }
    
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    
    public virtual User? User { get; set; }
    public virtual Job? Job { get; set; }
}