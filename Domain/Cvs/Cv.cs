using Domain._Shared.Entities;
using Domain._Shared.Entities.Interfaces;
using Domain._Shared.ValueEntities;
using Domain.Categories;
using Domain.Cvs.ValueEntities;
using Domain.Entities.Users;
using Domain.Locations;
using Domain.Users;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Cvs;

public class Cv : BaseEntity, IPublicOrPrivateEntity
{
    public static CvValidator Validator { get; } = new();
    
    public static Result<Cv> Create(long userId, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord,
        ICollection<EducationRecord> educationRecords, ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        var cv = new Cv(userId, salaryRecord, employmentTypeRecord, educationRecords, workRecords, skills);

        var validationResult = Validator.Validate(cv);

        return validationResult.IsValid ? cv : Result<Cv>.Invalid(validationResult.AsErrors());
    }
    
    private Cv(long userId, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord,
        ICollection<EducationRecord> educationRecords,
        ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        UserId = userId;
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
        _educationRecords = educationRecords.ToList();
        _workRecords = workRecords.ToList();
        _skills = skills.ToList();
    }
    
    public long UserId { get; private set; }
    
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


    private List<EducationRecord> _educationRecords;
    public IReadOnlyCollection<EducationRecord> EducationRecords => _educationRecords.AsReadOnly();

    public Result SetEducationRecords(ICollection<EducationRecord> newValues)
    {
        var oldValues = _educationRecords;
        _educationRecords = newValues.ToList();

        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _educationRecords = oldValues;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }


    private List<WorkRecord> _workRecords;
    public IReadOnlyCollection<WorkRecord> WorkRecords => _workRecords.AsReadOnly();

    public Result SetWorkRecords(ICollection<WorkRecord> newValues)
    {
        var oldValues = _workRecords;
        _workRecords = newValues.ToList();

        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _workRecords = oldValues;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }


    private List<string> _skills;
    public IReadOnlyCollection<string> Skills => _skills.AsReadOnly();

    public Result SetSkills(ICollection<string> newValues)
    {
        var oldValues = _skills;
        _skills = newValues.ToList();

        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            _skills = oldValues;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
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
    
    public virtual User? User { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }

}