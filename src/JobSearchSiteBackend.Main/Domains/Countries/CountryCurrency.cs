using System.Collections.Immutable;
using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains._Shared.Enums;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.JobContractTypes;
using JobSearchSiteBackend.Main.Domains.Jobs;
using JobSearchSiteBackend.Main.Domains.Locations;

namespace JobSearchSiteBackend.Main.Domains.Countries;

public class CountryCurrency : IEntityWithId
{
    public static readonly ImmutableArray<CountryCurrency> AllValues =
    [
        new(1, 1, 1)
    ];
    
    private CountryCurrency(long id, long countryId, long currencyId)
    {
        Id = id;
        CountryId = countryId;
        CurrencyId = currencyId;
    }
    
    public long Id { get; private set; }
    public long CountryId { get; private set; }
    public Country? Country { get; private set; }
    public long CurrencyId { get; private set; }
    public Currency? Currency { get; private set; }
    
    public ICollection<JobPublicationInterval>? JobPublicationIntervals { get; private set; }
}