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

public class Job : EntityBase
{
    public static JobValidator Validator { get; } = new();

    public static Result<Job> Create(long companyId, long categoryId, string title, string description, bool isPublic,
        DateTime dateTimePublishedUtc, DateTime dateTimeExpiringUtc, ICollection<string> responsibilities,
        ICollection<string> requirements, ICollection<string> advantages, SalaryRecord salaryRecord,
        EmploymentTypeRecord employmentTypeRecord)
    {
        var job = new Job(companyId, categoryId, title, description, isPublic, dateTimePublishedUtc, dateTimeExpiringUtc,
            responsibilities, requirements, advantages, salaryRecord, employmentTypeRecord);

        var validationResult = Validator.Validate(job);

        return validationResult.IsValid ? job : Result<Job>.Invalid(validationResult.AsErrors());
    }

    private Job(long companyId, long categoryId, string title, string description, bool isPublic, DateTime dateTimePublishedUtc,
        DateTime dateTimeExpiringUtc, ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        CompanyId = companyId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        IsPublic = isPublic;
        DateTimePublishedUtc = dateTimePublishedUtc;
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

    public string Description { get; private set; }

    public DateTime DateTimePublishedUtc { get; private set; }

    public DateTime DateTimeExpiringUtc { get; private set; }

    public SalaryRecord? SalaryRecord { get; private set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; private set; }

    public IReadOnlyCollection<string> Responsibilities { get; private set; }

    public IReadOnlyCollection<string> Requirements { get; private set; }

    public IReadOnlyCollection<string> Advantages { get; private set; }

    public bool IsPublic { get; private set; }

    public virtual Company? Company { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
    public virtual ICollection<JobContractType>? JobContractTypes { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }

    public virtual ICollection<UserProfile>? UsersWhoBookmarked { get; set; }

    public virtual ICollection<JobFolder>? JobFolders { get; set; }
}