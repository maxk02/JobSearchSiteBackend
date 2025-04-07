using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Companies;
using Core.Domains.JobContractTypes;
using Core.Domains.Locations;

namespace Core.Domains.Countries;

public class Country : IEntityWithId
{
    public static readonly ImmutableArray<Country> AllValues =
    [
        new Country(1, "POL"),
        new Country(2, "DEU"),
        new Country(3, "FRA"),
    ];
    
    private Country(long id, string code)
    {
        Id = id;
        Code = code;
    }
    
    public long Id { get; set; }
    public string Code { get; private set; }
    
    public ICollection<JobContractType>? JobContractTypes { get; set; }
    public ICollection<Location>? Locations { get; set; }
    public ICollection<Company>? Companies { get; set; }
}