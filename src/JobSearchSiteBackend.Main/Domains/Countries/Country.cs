using System.Collections.Immutable;
using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains._Shared.Enums;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.Countries.Enums;
using JobSearchSiteBackend.Main.Domains.JobContractTypes;
using JobSearchSiteBackend.Main.Domains.Locations;

namespace JobSearchSiteBackend.Main.Domains.Countries;

public class Country : IEntityWithId
{
    public static readonly ImmutableArray<Country> AllValues =
    [
        new Country((long)CountryIdsEnum.Poland, "POL"),
        new Country((long)CountryIdsEnum.Germany, "DEU"),
        new Country((long)CountryIdsEnum.France, "FRA"),
    ];
    
    private Country(long id, string code)
    {
        Id = id;
        Code = code;
    }
    
    public long Id { get; private set; }
    public string Code { get; private set; }
    
    public ICollection<JobContractType>? JobContractTypes { get; set; }
    public ICollection<Location>? Locations { get; set; }
    public ICollection<Company>? Companies { get; set; }
    public ICollection<CountryCurrency>? CountryCurrencies { get; private set; }
}