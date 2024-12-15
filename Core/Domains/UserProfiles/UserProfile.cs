using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Companies;
using Core.Domains.CompanyPermissions.UserCompanyCompanyPermissions;
using Core.Domains.Cvs;
using Core.Domains.FolderPermissions.UserFolderFolderPermissions;
using Core.Domains.JobApplications;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.UserProfiles;

public class UserProfile : BaseEntity, IPublicOrPrivateEntity
{
    public static UserProfileValidator ProfileValidator { get; } = new();

    public static Result<UserProfile> Create(long accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone, string? bio)
    {
        var user = new UserProfile(accountId, firstName, middleName, lastName, dateOfBirth, email, phone, bio);

        var validationResult = ProfileValidator.Validate(user);

        return validationResult.IsValid ? user : Result<UserProfile>.Invalid(validationResult.AsErrors());
    }

    private UserProfile(long accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone, string? bio)
    {
        Id = accountId;
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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

        var validationResult = ProfileValidator.Validate(this);
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