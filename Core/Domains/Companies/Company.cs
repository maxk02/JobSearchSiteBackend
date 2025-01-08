using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.CompanyClaims;
using Core.Domains.Countries;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Companies;

public class Company : IEntityWithId
{
    public Company(string name, string? description, bool isPublic, long countryId)
    {
        CountryId = countryId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
    }
    
    public long Id { get; private set; }
    
    public long CountryId { get; private set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsPublic { get; set; }
    
    public Country? Country { get; private set; }

    public ICollection<JobFolder>? JobFolders { get; set; }
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
}