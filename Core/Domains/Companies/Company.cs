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
    
    public long Id { get; set; }
    
    public long CountryId { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public bool IsPublic { get; private set; }
    
    public virtual Country? Country { get; set; }

    public virtual ICollection<JobFolder>? JobFolders { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    
    public virtual ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public virtual ICollection<UserCompanyClaim>? UserCompanyPermissions { get; set; }
}