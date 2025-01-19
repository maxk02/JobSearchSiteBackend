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
    
    // ef core
    private UserProfile(string firstName, string? middleName, string lastName,
        DateOnly? dateOfBirth, string email)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
    }
    
    public long Id { get; private set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    
    public string Email { get; set; }
    
    public Phone? Phone { get; set; }
    
    
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    public ICollection<Cv>? Cvs { get; set; }
    
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<Job>? BookmarkedJobs { get; set; }
    public ICollection<Company>? BookmarkedCompanies { get; set; }

    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
    
    public ICollection<UserSession>? UserSessions { get; set; }
}