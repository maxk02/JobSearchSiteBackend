using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains._Shared.Enums;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.Locations;

namespace JobSearchSiteBackend.Core.Domains.Countries;

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