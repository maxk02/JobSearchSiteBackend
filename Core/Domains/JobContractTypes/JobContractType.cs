using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Countries;
using Core.Domains.Jobs;

namespace Core.Domains.JobContractTypes;

public class JobContractType : IEntityWithId
{
    private JobContractType(long countryId, string namePl)
    {
        CountryId = countryId;
        NamePl = namePl;
    }
    
    public long Id { get; private set; }
    
    public long CountryId { get; private set; }
    
    public string NamePl { get; private set; }
    
    public Country? Country { get; set; }
    public ICollection<Job>? Jobs { get; set; }
}