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
    
    public long Id { get; }
    
    public long CountryId { get; }
    
    public string Name { get; }
    
    public virtual Country? Country { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
}