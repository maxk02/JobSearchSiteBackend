using Domain._Shared.Entities;
using Domain._Shared.Entities.Interfaces;
using Domain._Shared.ValueEntities;
using Domain.Companies;
using Domain.CompanyPermissions.UserCompanyCompanyPermissions;
using Domain.Cvs;
using Domain.FolderPermissions.UserFolderFolderPermissions;
using Domain.JobApplications;
using Domain.Jobs;
using Domain.PersonalFiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Users;

public class User : BaseEntity, IPublicOrPrivateEntity
{
    public static UserValidator Validator { get; } = new();

    public static Result<User> Create(string accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone, string? bio)
    {
        var user = new User(accountId, firstName, middleName, lastName, dateOfBirth, email, phone, bio);

        var validationResult = Validator.Validate(user);

        return validationResult.IsValid ? user : Result<User>.Invalid(validationResult.AsErrors());
    }

    private User(string accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone, string? bio)
    {
        AccountId = accountId;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
        Bio = bio;
    }
    
    public string AccountId { get; private set; }

    public string FirstName { get; private set; }

    public Result SetFirstName(string newValue)
    {
        var oldValue = FirstName;
        FirstName = newValue;

        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            FirstName = oldValue;
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }

    public Phone? Phone { get; private set; }

    public Result SetPhone(Phone? newValue)
    {
        var oldValue = Phone;
        Phone = newValue;

        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Phone = oldValue;
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    //

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

    
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
    
    
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
    public virtual ICollection<Job>? BookmarkedJobs { get; set; }
    public virtual ICollection<Company>? BookmarkedCompanies { get; set; }

    public virtual ICollection<UserCompanyCompanyPermission>? UserCompanyCompanyPermissions { get; set; }
    public virtual ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}