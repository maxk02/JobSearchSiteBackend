using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Applications;

public sealed class Application : BaseEntity
{
    private static ApplicationValidator Validator { get; } = new ApplicationValidator();
    
    public static Result<Application> Create(int userId, int jobId)
    {
        var application = new Application(userId, jobId);

        var result = Validator.Validate(application);
        
        return result.IsValid ? Result.Success(application) : Result.Failure<Application>(result.Errors);
    }
    
    private Application(int userId, int jobId)
    {
        UserId = userId;
        JobId = jobId;
    }
    
    public User? User { get; set; }
    public int UserId { get; private set; }
    
    public Job? Job { get; set; }
    public int JobId { get; private set; }
    
    // public virtual IList<File>? Files { get; set; }
}