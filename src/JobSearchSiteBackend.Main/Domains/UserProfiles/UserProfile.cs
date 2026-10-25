using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains.Accounts;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.JobApplications;
using JobSearchSiteBackend.Main.Domains.PersonalFiles;
using JobSearchSiteBackend.Main.Domains.Accounts;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles;

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
    
    public ICollection<UserJobApplicationBookmark>? UserJobApplicationBookmarks { get; set; }

    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    
    public ICollection<UserAvatar>? UserAvatars { get; set; }
    
    public ICollection<Company>? CompaniesWhereEmployed { get; set; }
    
    public ICollection<CompanyBalanceTransaction>? CompanyBalanceTransactions { get; set; }

    public ICollection<CompanyEmployeeInvitation>? CompanyEmployeeInvitationsSent { get; set; }
    public ICollection<CompanyEmployeeInvitation>? CompanyEmployeeInvitationsReceived { get; set; }
}