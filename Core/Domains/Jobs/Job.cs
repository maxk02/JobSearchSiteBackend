using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Categories;
using Core.Domains.Companies;
using Core.Domains.JobApplications;
using Core.Domains.JobContractTypes;
using Core.Domains.JobFolders;
using Core.Domains.Locations;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Jobs;

public class Job : EntityBase, IPublicOrPrivateEntity
{
    public static JobValidator Validator { get; } = new();

    public static Result<Job> Create(long companyId, long categoryId, string title, string description,
        DateTime dateTimeExpiringUtc, ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        var job = new Job(companyId, categoryId, title, description, dateTimeExpiringUtc,
            responsibilities, requirements, advantages, salaryRecord, employmentTypeRecord);

        var validationResult = Validator.Validate(job);

        return validationResult.IsValid ? job : Result<Job>.Invalid(validationResult.AsErrors());
    }

    private Job(long companyId, long categoryId, string title, string description,
        DateTime dateTimeExpiringUtc, ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        CompanyId = companyId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        DateTimeExpiringUtc = dateTimeExpiringUtc;
        Responsibilities = responsibilities.ToList();
        Requirements = requirements.ToList();
        Advantages = advantages.ToList();
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
    }
    
    public long CompanyId { get; private set; }
    
    public long CategoryId { get; private set; }
    public Result SetCategoryId(long newValue)
    {
        long oldValue = CategoryId;
        CategoryId = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            CategoryId = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public string Title { get; private set; }
    public Result SetTitle(string newValue)
    {
        var oldValue = Title;
        Title = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Title = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }

    public string Description { get; private set; }
    public Result SetDescription(string newValue)
    {
        var oldValue = Description;
        Description = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Description = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public DateTime DateTimeExpiringUtc { get; private set; }
    public Result SetDateTimeExpiringUtc(DateTime newValue)
    {
        var oldValue = DateTimeExpiringUtc;
        DateTimeExpiringUtc = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            DateTimeExpiringUtc = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public SalaryRecord? SalaryRecord { get; private set; }
    public Result SetSalaryRecord(SalaryRecord? newValue)
    {
        var oldValue = SalaryRecord;
        SalaryRecord = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            SalaryRecord = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public EmploymentTypeRecord? EmploymentTypeRecord { get; private set; }
    public Result SetEmploymentTypeRecord(EmploymentTypeRecord? newValue)
    {
        var oldValue = EmploymentTypeRecord;
        EmploymentTypeRecord = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            EmploymentTypeRecord = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public IReadOnlyCollection<string> Responsibilities { get; private set; }
    
    public IReadOnlyCollection<string> Requirements { get; private set; }
    
    public IReadOnlyCollection<string> Advantages { get; private set; }
    
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
    
    public virtual Company? Company { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
    public virtual ICollection<JobContractType>? JobContractTypes { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    
    public virtual ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    
    public virtual ICollection<JobFolder>? JobFolders { get; set; }
}