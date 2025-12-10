using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles;

public class UserProfile : IEntityWithId
{
    public UserProfile(long id, string firstName, string lastName,
        string? phone, bool isReceivingApplicationStatusUpdates)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        IsReceivingApplicationStatusUpdates = isReceivingApplicationStatusUpdates;
    }
    
    public long Id { get; private set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string? Phone { get; set; }
    
    public bool IsReceivingApplicationStatusUpdates { get; set; }
    
    public MyIdentityUser? Account { get; set; }
    
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<UserJobBookmark>? UserJobBookmarks { get; set; }
    // public ICollection<Company>? BookmarkedCompanies { get; set; }
    
    public ICollection<UserJobApplicationBookmark>? UserJobApplicationBookmarks { get; set; }

    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
    
    public ICollection<UserAvatar>? UserAvatars { get; set; }
    
    public ICollection<Company>? CompaniesWhereEmployed { get; set; }
    
    public ICollection<CompanyBalanceTransaction>? CompanyBalanceTransactions { get; set; }
}