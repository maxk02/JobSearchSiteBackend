using Domain._Shared.Entities;
using Domain.Jobs;
using Domain.PersonalFiles;
using Domain.Users;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.JobApplications;

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