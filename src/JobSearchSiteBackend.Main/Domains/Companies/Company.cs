using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.Countries;
using JobSearchSiteBackend.Main.Domains.Jobs;
using JobSearchSiteBackend.Main.Domains.UserProfiles;

namespace JobSearchSiteBackend.Main.Domains.Companies;

public class Company : IEntityWithId
{
    public Company(string name, string? description, bool isPublic,
        long countryId, string countrySpecificFieldsJson)
    {
        CountryId = countryId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
        CountrySpecificFieldsJson = countrySpecificFieldsJson;
        IsVerified = true;
    }
    
    public long Id { get; set; }
    
    // public Guid VersionId { get; set; } = Guid.NewGuid();
    //
    // public Guid? VersionIdSyncedWithSearch { get; set; } = null;
    
    public bool IsDeleted { get; set; }
    
    public long CountryId { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsPublic { get; set; }
    
    public string CountrySpecificFieldsJson { get; set; }
    
    public bool IsVerified { get; set; }
    
    public Country? Country { get; set; }
    
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
    
    public ICollection<CompanyAvatar>? CompanyAvatars { get; set; }
    
    public ICollection<CompanyBalanceTransaction>? CompanyBalanceTransactions { get; set; }

    public ICollection<CompanyEmployeeInvitation>? CompanyEmployeeInvitations { get; set; }
    
    public ICollection<UserProfile>? Employees { get; set; }
}