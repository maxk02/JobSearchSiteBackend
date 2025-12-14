using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public class Location : IEntityWithId
{
    public Location(long id, long countryId, string name, string fullName,
        bool isConcrete, string? code, string? descriptionPl)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        FullName = fullName;
        IsConcrete = isConcrete;
        Code = code;
        DescriptionPl = descriptionPl;
    }
    
    public long Id { get; private set; }
    public long CountryId { get; private set; }
    public string Name { get; private set; }
    public string FullName { get; private set; }
    public bool IsConcrete { get; private set; }
    public string? Code { get; private set; }
    public string? DescriptionPl { get; private set; }

    
    public Country? Country { get; set; }
    public ICollection<UserProfile>? Users { get; set; }
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<LocationRelation>? RelationsWhereThisIsDescendant { get; set; }
    public ICollection<LocationRelation>? RelationsWhereThisIsAncestor { get; set; }
}