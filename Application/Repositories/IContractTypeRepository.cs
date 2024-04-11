using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IContractTypeRepository : IBaseRepository<ContractType>
{
    Task<IList<ContractType>> GetListForCountryId(long countryId);
}