using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Accounts;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.JobApplications;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;

namespace Core.Domains.UserProfiles;

public class UserProfile : IEntityWithId
{
    public UserProfile(long accountId, string firstName, string lastName, string email,
        string? phone, string? avatarLink = null)
    {
        Id = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        AvatarLink = avatarLink;
    }
    
    public long Id { get; private set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? AvatarLink { get; set; }
    
    
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<Job>? BookmarkedJobs { get; set; }
    public ICollection<Company>? BookmarkedCompanies { get; set; }
    
    public ICollection<Job>? LastManagedJobs { get; set; }
    public ICollection<JobFolder>? LastManagedJobFolders { get; set; }
    public ICollection<JobApplication>? BookmarkedJobApplications { get; set; }

    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
    
    public ICollection<UserSession>? UserSessions { get; set; }
}