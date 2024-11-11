using Domain.Entities.Applications;
using Domain.Entities.Categories;
using Domain.Entities.Companies;
using Domain.Entities.ContractTypes;
using Domain.Entities.Locations;
using Domain.Entities.Tags;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Domain.Shared.ValueEntities;
using Shared.Results;

namespace Domain.Entities.Jobs;

public class Job : BaseEntity, IHideableEntity
{
    public static JobValidator Validator { get; } = new();

    public static Result<Job> Create(long companyId, long categoryId, string title, string description,
        DateTime dateTimeExpiringUtc,
        ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, ICollection<string> addresses,
        SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        var job = new Job(companyId, categoryId, title, description, dateTimeExpiringUtc,
            responsibilities, requirements, advantages, addresses, salaryRecord, employmentTypeRecord);

        var validationResult = Validator.Validate(job);

        return validationResult.IsValid ? Result.Success(job) : Result.Failure<Job>(validationResult.Errors);
    }

    private Job(long companyId, long categoryId, string title, string description, DateTime dateTimeExpiringUtc,
        ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, ICollection<string> addresses,
        SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        CompanyId = companyId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        DateTimeExpiringUtc = dateTimeExpiringUtc;
        _responsibilities = responsibilities.ToList();
        _requirements = requirements.ToList();
        _advantages = advantages.ToList();
        _addresses = addresses.ToList();
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }

    private List<string> _responsibilities;
    public IReadOnlyCollection<string> Responsibilities => _responsibilities.AsReadOnly();
    public Result SetResponsibilities(ICollection<string> newValues)
    {
        var oldValues = _responsibilities;
        _responsibilities = newValues.ToList();
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _responsibilities = oldValues;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    private List<string> _requirements;
    public IReadOnlyCollection<string> Requirements => _requirements.AsReadOnly();
    public Result SetRequirements(ICollection<string> newValues)
    {
        var oldValues = _requirements;
        _requirements = newValues.ToList();
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _requirements = oldValues;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    private List<string> _advantages;
    public IReadOnlyCollection<string> Advantages => _advantages.AsReadOnly();
    public Result SetAdvantages(ICollection<string> newValues)
    {
        var oldValues = _advantages;
        _advantages = newValues.ToList();
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _advantages = oldValues;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    private List<string> _addresses;
    public IReadOnlyCollection<string> Addresses => _addresses.AsReadOnly();
    public Result SetAddresses(ICollection<string> newValues)
    {
        var oldValues = _addresses;
        _addresses = newValues.ToList();
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _addresses = oldValues;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public bool IsHidden { get; private set; }
    public Result MakeHidden()
    {
        IsHidden = true;
        return Result.Success();
    }
    public Result MakeVisible()
    {
        IsHidden = false;
        return Result.Success();
    }

    // public bool IsExpired { get; set; } //todo
    
    public virtual Company? Company { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Application>? MyApplications { get; set; }
    public virtual ICollection<ContractType>? ContractTypes { get; set; }
    public virtual ICollection<Tag>? Tags { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    
    public virtual ICollection<UserJobBookmark>? UserJobBookmarks { get; set; }
}