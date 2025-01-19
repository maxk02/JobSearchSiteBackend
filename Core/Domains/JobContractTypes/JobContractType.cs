using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Countries;
using Core.Domains.Jobs;

namespace Core.Domains.JobContractTypes;

public class JobContractType : IEntityWithId
{
    private JobContractType(long countryId, string name)
    {
        CountryId = countryId;
        Name = name;
    }
    
    public long Id { get; private set; }
    
    public long CountryId { get; private set; }
    
    public string Name { get; private set; }
    
    public Country? Country { get; set; }
    public ICollection<Job>? Jobs { get; set; }
}