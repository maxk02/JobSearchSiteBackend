using Domain.Entities.Jobs;
using Domain.Entities.PersonalFiles;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Entities.JobApplications;

public class JobApplication : BaseEntity
{
    public static JobApplicationValidator Validator { get; } = new();
    
    public static Result<JobApplication> Create(int userId, int jobId)
    {
        var application = new JobApplication(userId, jobId);

        var validationResult = Validator.Validate(application);
        
        return validationResult.IsValid ? application : Result<JobApplication>.Invalid(validationResult.AsErrors());
    }
    
    private JobApplication(int userId, int jobId)
    {
        UserId = userId;
        JobId = jobId;
    }
    
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    
    public virtual Job? Job { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
}