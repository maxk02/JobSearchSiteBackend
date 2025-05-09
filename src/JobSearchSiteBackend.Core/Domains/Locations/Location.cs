using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public class Location : IEntityWithId
{
    private Location(long countryId, string name, bool isConcrete, string? description, string? code)
    {
        CountryId = countryId;
        Name = name;
        IsConcrete = isConcrete;
        Description = description;
        Code = code;
    }
    
    public long Id { get; private set; }
    public long CountryId { get; private set; }
    public string Name { get; private set; }
    public bool IsConcrete { get; private set; }
    public string? Description { get; private set; }
    public string? Code { get; private set; }

    
    public Country? Country { get; set; }
    public ICollection<UserProfile>? Users { get; set; }
    public ICollection<Job>? Jobs { get; set; }
    
    public ICollection<LocationRelation>? RelationsWhereThisIsDescendant { get; set; }
    public ICollection<LocationRelation>? RelationsWhereThisIsAncestor { get; set; }
}