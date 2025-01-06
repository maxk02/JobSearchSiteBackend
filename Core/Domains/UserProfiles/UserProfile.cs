using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Companies;
using Core.Domains.CompanyPermissions.UserCompanyPermissions;
using Core.Domains.Cvs;
using Core.Domains.JobApplications;
using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.UserProfiles;

public class UserProfile : EntityBase
{
    public static UserProfileValidator ProfileValidator { get; } = new();

    public static Result<UserProfile> Create(long accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone)
    {
        var user = new UserProfile(accountId, firstName, middleName, lastName, dateOfBirth, email, phone);

        var validationResult = ProfileValidator.Validate(user);

        return validationResult.IsValid ? user : Result<UserProfile>.Invalid(validationResult.AsErrors());
    }

    private UserProfile(long accountId, string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email, Phone? phone)
    {
        Id = accountId;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
    }

    public string FirstName { get; private set; }

    public string? MiddleName { get; private set; }

    public string LastName { get; private set; }
    
    public DateOnly? DateOfBirth { get; private set; }
    
    public string Email { get; private set; }
    
    public Phone? Phone { get; private set; }
    
    
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
    
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
    public virtual ICollection<Job>? BookmarkedJobs { get; set; }
    public virtual ICollection<Company>? BookmarkedCompanies { get; set; }

    public virtual ICollection<UserCompanyPermission>? UserCompanyPermissions { get; set; }
    public virtual ICollection<UserJobFolderPermission>? UserFolderPermissions { get; set; }
}