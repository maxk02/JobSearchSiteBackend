using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobApplications;

public class JobApplication : EntityBase
{
    public static JobApplicationValidator Validator { get; } = new();
    
    public static Result<JobApplication> Create(long userId, long jobId, string status)
    {
        var application = new JobApplication(userId, jobId, status);

        var validationResult = Validator.Validate(application);
        
        return validationResult.IsValid ? application : Result<JobApplication>.Invalid(validationResult.AsErrors());
    }
    
    private JobApplication(long userId, long jobId, string status)
    {
        UserId = userId;
        JobId = jobId;
        Status = status;
    }
    
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    public string Status { get; set; }
    
    public Result SetStatus(string newValue)
    {
        var oldValue = Status;
        Status = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Status = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public virtual Job? Job { get; set; }
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
}