using Domain.Entities.Applications;
using Domain.Entities.Categories;
using Domain.Entities.Locations;
using Domain.Entities.Users.ValueEntities;
using Domain.Shared.Entities;
using Domain.Shared.ValueEntities;
using Shared.Results;

namespace Domain.Entities.Users;

public class User : BaseEntity, IHideableEntity
{
    public static UserValidator Validator { get; } = new();

    public static Result<User> Create(string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, string? phone, string? bio,
        SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord,
        ICollection<EducationRecord> educationRecords,
        ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        var user = new User(firstName, middleName, lastName, dateOfBirth, email, phone, bio,
                salaryRecord, employmentTypeRecord, educationRecords, workRecords, skills);

        var validationResult = Validator.Validate(user);

        return validationResult.IsValid ? Result.Success(user) : Result.Failure<User>(validationResult.Errors);
    }

    private User(string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, string? phone, string? bio,
        SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord,
        ICollection<EducationRecord> educationRecords,
        ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
        Bio = bio;
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
        _educationRecords = educationRecords.ToList();
        _workRecords = workRecords.ToList();
        _skills = skills.ToList();
    }
    
    public string FirstName { get; private set; }
    public Result SetFirstName(string newValue)
    {
        var oldValue = FirstName;
        FirstName = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            FirstName = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public string? MiddleName { get; private set; }
    public Result SetMiddleName(string? newValue)
    {
        var oldValue = MiddleName;
        MiddleName = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            MiddleName = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public string LastName { get; private set; }
    public Result SetLastName(string newValue)
    {
        var oldValue = LastName;
        LastName = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            LastName = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }

    public DateOnly? DateOfBirth { get; private set; }
    public Result SetDateOfBirth(DateOnly newValue)
    {
        var oldValue = DateOfBirth;
        DateOfBirth = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            DateOfBirth = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public string Email { get; private set; }
    public Result SetEmail(string newValue)
    {
        var oldValue = Email;
        Email = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Email = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public string? Phone { get; private set; }
    public Result SetPhone(string? newValue)
    {
        var oldValue = Phone;
        Phone = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Phone = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public string? Bio { get; private set; }
    public Result SetBio(string? newValue)
    {
        var oldValue = Bio;
        Bio = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Bio = oldValue;
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
            return Result.Failure(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
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

    
    public virtual IList<Location>? Locations { get; set; }
    public virtual IList<Category>? Categories { get; set; }
    public virtual IList<FileInfo>? MyFiles { get; set; }
    public virtual IList<Application>? MyApplications { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}