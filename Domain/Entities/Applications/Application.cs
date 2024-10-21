using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Domain.Shared.Errors;
using Domain.Shared.Results;
using Domain.Validators;
using Domain.Validators.LongIntValidator;

namespace Domain.Entities.Applications;

public sealed class Application : BaseEntity
{
    public static Result<Application> Create(int userId, int jobId)
    {
        List<DomainLayerError> errors = [];
        
        errors.AddRange(UserIdValidator(userId).ReturnErrors());
        errors.AddRange(JobIdValidator(jobId).ReturnErrors());

        if (errors.Count > 0)
            return Result.Failure<Application>(errors);
        
        
        var application = new Application(userId, jobId);

        return Result.Success(application);
    }
    
    private Application() {}

    private Application(int userId, int jobId)
    {
        
    }
    
    public User? User { get; set; }
    public int UserId { get; private set; }
    private static ISealedValidator UserIdValidator(long userId)
    {
        var validator = new LongIntValidator(userId, nameof(Application), nameof(UserId))
            .MustBeEqualOrMoreThan(1);

        return validator;
    }
    
    public Job? Job { get; set; }
    public int JobId { get; private set; }
    private static ISealedValidator JobIdValidator(long jobId)
    {
        var validator = new LongIntValidator(jobId, nameof(Application), nameof(JobId))
            .MustBeEqualOrMoreThan(1);

        return validator;
    }
    
    // public virtual IList<File>? Files { get; set; }
}