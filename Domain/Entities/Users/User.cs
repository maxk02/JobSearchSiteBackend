using Domain.Entities.Companies;
using Domain.Entities.Cvs;
using Domain.Entities.JobApplications;
using Domain.Entities.Jobs;
using Domain.Entities.PersonalFiles;
using Domain.Entities.UserCompanyCompanyPermissions;
using Domain.Entities.UserFolderFolderPermissions;
using Domain.Shared.Entities;
using Domain.Shared.Entities.Interfaces;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Entities.Users;

public class User : BaseEntity, IPublicOrPrivateEntity
{
    public static UserValidator Validator { get; } = new();

    public static Result<User> Create(string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, string? phone, string? bio)
    {
        var user = new User(firstName, middleName, lastName, dateOfBirth, email, phone, bio);

        var validationResult = Validator.Validate(user);

        return validationResult.IsValid ? user : Result<User>.Invalid(validationResult.AsErrors());
    }

    private User(string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, string? phone, string? bio)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
        Bio = bio;
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

    public string? Phone { get; private set; }

    public Result SetPhone(string? newValue)
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
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }

    public virtual ICollection<Job>? BookmarkedJobs { get; set; }
    public virtual ICollection<Company>? BookmarkedCompanies { get; set; }

    public virtual ICollection<UserCompanyCompanyPermission>? UserCompanyCompanyPermissions { get; set; }
    public virtual ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}