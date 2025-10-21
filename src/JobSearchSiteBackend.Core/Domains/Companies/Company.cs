using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class Company : IEntityWithId, IEntityWithUpdDelDate
{
    public Company(string name, string? description, bool isPublic, long countryId, string? logoLink)
    {
        CountryId = countryId;
        LogoLink = logoLink;
        Name = name;
        Description = description;
        IsPublic = isPublic;
    }
    
    public long Id { get; set; }
    
    public DateTime DateTimeUpdatedUtc { get; set; }
    public bool IsDeleted { get; set; }
    
    public long CountryId { get; set; }
    
    public string? LogoLink { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public decimal Balance { get; set; } = decimal.Zero;
    
    public bool IsPublic { get; set; }
    
    public Country? Country { get; set; }

    public ICollection<JobFolder>? JobFolders { get; set; }
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
}