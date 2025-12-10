using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.Jobs;

namespace JobSearchSiteBackend.Core.Domains.JobContractTypes;

public class JobContractType : IEntityWithId
{
    public static readonly ImmutableArray<JobContractType> AllValues =
    [
        new JobContractType(1, 1, "Umowa o pracę"),
        new JobContractType(2, 1, "Umowa o dzieło"),
        new JobContractType(3, 1, "Umowa zlecenie"),
        new JobContractType(4, 1, "Kontrakt B2B"),
        new JobContractType(5, 1, "Umowa o pracę tymczasową"),
        new JobContractType(6, 1, "Umowa agencyjna"),
        new JobContractType(7, 1, "Umowa o staż/praktykę"),
        new JobContractType(8, 1, "Umowa na zastępstwo")
    ];
    
    private JobContractType(long id, long countryId, string namePl)
    {
        Id = id;
        CountryId = countryId;
        NamePl = namePl;
    }
    
    public long Id { get; private set; }
    
    public long CountryId { get; private set; }
    
    public string NamePl { get; private set; }
    
    public Country? Country { get; set; }
    public ICollection<Job>? Jobs { get; set; }
}