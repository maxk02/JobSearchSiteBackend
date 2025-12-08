using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class Company : IEntityWithId, IEntityWithSearchSync
{
    public Company(string name, string? description, bool isPublic,
        long countryId, string countrySpecificFieldsJson)
    {
        CountryId = countryId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
        CountrySpecificFieldsJson = countrySpecificFieldsJson;
    }
    
    public long Id { get; set; }
    
    public DateTime DateTimeUpdatedUtc { get; set; }
    
    public DateTime? DateTimeSyncedWithSearchUtc { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public long CountryId { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsPublic { get; set; }
    
    public string CountrySpecificFieldsJson { get; set; }
    
    public Country? Country { get; set; }

    public ICollection<JobFolder>? JobFolders { get; set; }
    
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    
    public ICollection<CompanyAvatar>? CompanyAvatars { get; set; }
    
    public ICollection<CompanyBalanceTransaction>? CompanyBalanceTransactions { get; set; }
    
    public ICollection<UserProfile>? Employees { get; set; }
}