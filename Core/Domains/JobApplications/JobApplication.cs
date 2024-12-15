using Core.Domains._Shared.Entities;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobApplications;

public class JobApplication : BaseEntity
{
    public static JobApplicationValidator Validator { get; } = new();
    
    public static Result<JobApplication> Create(long userId, long jobId)
    {
        var application = new JobApplication(userId, jobId);

        var validationResult = Validator.Validate(application);
        
        return validationResult.IsValid ? application : Result<JobApplication>.Invalid(validationResult.AsErrors());
    }
    
    private JobApplication(long userId, long jobId)
    {
        UserId = userId;
        JobId = jobId;
    }
    
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    
    public virtual Job? Job { get; set; }
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
}