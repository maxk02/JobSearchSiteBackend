using Domain.Entities.Jobs;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Applications;

public class Application : BaseEntity
{
    public static ApplicationValidator Validator { get; } = new();
    
    public static Result<Application> Create(int userId, int jobId)
    {
        var application = new Application(userId, jobId);

        var validationResult = Validator.Validate(application);
        
        return validationResult.IsValid ? Result.Success(application) : Result.Failure<Application>(validationResult.Errors);
    }
    
    private Application(int userId, int jobId)
    {
        UserId = userId;
        JobId = jobId;
    }
    
    public int UserId { get; private set; }
    public int JobId { get; private set; }
    
    public virtual Job? Job { get; set; }
    public virtual User? User { get; set; }
    public virtual IList<FileInfo>? FileInfos { get; set; }
}