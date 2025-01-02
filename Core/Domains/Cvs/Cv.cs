using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Categories;
using Core.Domains.Cvs.ValueEntities;
using Core.Domains.Locations;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Cvs;

public class Cv : EntityBase, IPublicOrPrivateEntity
{
    public static CvValidator Validator { get; } = new();
    
    public static Result<Cv> Create(long userId, SalaryRecord? salaryRecord, EmploymentTypeRecord? employmentTypeRecord,
        ICollection<EducationRecord> educationRecords, ICollection<WorkRecord> workRecords,
        ICollection<string> skills)
    {
        var cv = new Cv(userId, salaryRecord, employmentTypeRecord, educationRecords, workRecords, skills);

        var validationResult = Validator.Validate(cv);

        return validationResult.IsValid ? cv : Result<Cv>.Invalid(validationResult.AsErrors());
    }
    
    private Cv(long userId, SalaryRecord? salaryRecord, EmploymentTypeRecord? employmentTypeRecord,
        ICollection<EducationRecord> educationRecords,
        ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        UserId = userId;
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
        EducationRecords = [..educationRecords];
        WorkRecords = [..workRecords];
        Skills = [..skills];
    }
    
    public long UserId { get; private set; }
    
    public SalaryRecord? SalaryRecord { get; private set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; private set; }
    
    public IReadOnlyCollection<EducationRecord>? EducationRecords { get; private set; }
    
    public IReadOnlyCollection<WorkRecord>? WorkRecords { get; private set; }
    
    public IReadOnlyCollection<string>? Skills { get; private set; }
    
    public bool IsPublic { get; private set; }
    
    public Result MakePublic()
    {
        IsPublic = true;
        return Result.Success();
    }

    public Result MakePrivate()
    {
        IsPublic = false;
        return Result.Success();
    }
    
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }

}