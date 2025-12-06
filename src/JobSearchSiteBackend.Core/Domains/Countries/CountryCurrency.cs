using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains._Shared.Enums;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;

namespace JobSearchSiteBackend.Core.Domains.Countries;

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