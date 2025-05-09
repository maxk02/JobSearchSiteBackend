using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains._Shared.Enums;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.Locations;

namespace JobSearchSiteBackend.Core.Domains.Countries;

public class Country : IEntityWithId
{
    public static readonly ImmutableArray<Country> AllValues =
    [
        new Country(1, "POL"),
        new Country(2, "DEU"),
        new Country(3, "FRA"),
    ];

    public static readonly Dictionary<long, ImmutableArray<Currency>> CountryCurrencies =
        new()
        {
            [1] = [Currency.PLN],
        };
    
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