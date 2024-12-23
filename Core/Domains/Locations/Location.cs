using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.Countries;
using Core.Domains.Jobs;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Locations;

public class Location : BaseEntity
{
    public Location(long countryId, string name, ICollection<string> subdivisions, string? description, string? code)
    {
        CountryId = countryId;
        Name = name;
        Subdivisions = subdivisions;
        Description = description;
        Code = code;
    }
    
    public long CountryId { get; private set; }
    public string Name { get; private set; }
    public ICollection<string> Subdivisions { get; private set; }
    public string? Description { get; private set; }
    public string? Code { get; private set; }

    
    public virtual Country? Country { get; set; }
    public virtual ICollection<UserProfile>? Users { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
}