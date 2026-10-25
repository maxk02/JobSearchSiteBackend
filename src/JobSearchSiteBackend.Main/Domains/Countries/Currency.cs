using System.Collections.Immutable;
using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains._Shared.Enums;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.JobContractTypes;
using JobSearchSiteBackend.Main.Domains.Locations;

namespace JobSearchSiteBackend.Main.Domains.Countries;

public class Currency : IEntityWithId
{
    public static readonly ImmutableArray<Currency> AllValues =
    [
        new(1, "PLN"),
        new(2, "EUR"),
        new(3, "USD")
    ];
    
    private Currency(long id, string code)
    {
        Id = id;
        Code = code;
    }
    
    public long Id { get; set; }
    public string Code { get; private set; }
    
    public ICollection<CountryCurrency>? CountryCurrencies { get; private set; }
}