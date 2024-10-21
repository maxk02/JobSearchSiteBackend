using Domain.Entities;
using Domain.Entities.ContractTypes;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface IContractTypeRepository : IBaseRepository<ContractType>
{
    Task<IList<ContractType>> GetListForCountryId(long countryId);
}