using Core.Domains._Shared.Repositories;
using Shared.Result;

namespace Core.Domains.ContractTypes;

public interface IContractTypeRepository : IRepository<ContractType>
{
    public Task<ICollection<ContractType>> GetByCountryIdAsync(long countryId, CancellationToken cancellationToken = default);
}