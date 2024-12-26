using Core.Domains._Shared.Repositories;

namespace Core.Domains.JobContractTypes;

public interface IJobContractTypeRepository : IRepository<JobContractType>
{
    public Task<ICollection<JobContractType>> GetByCountryIdAsync(long countryId, CancellationToken cancellationToken = default);
}