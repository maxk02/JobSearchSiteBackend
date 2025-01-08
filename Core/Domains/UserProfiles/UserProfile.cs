using Core.Domains._Shared.EntityInterfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Accounts;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.Cvs;
using Core.Domains.JobApplications;
using Core.Domains.JobFolderClaims;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.UserProfiles;

public class UserProfile : IEntityWithId
{
    public UserProfile(long accountId, string firstName, string? middleName, string lastName,
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
    
    public long Id { get; set; }

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

    public virtual ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    public virtual ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
    
    public virtual ICollection<UserSession>? UserSessions { get; set; }
}