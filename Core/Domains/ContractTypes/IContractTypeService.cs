using Core.Domains.ContractTypes.UseCases.GetContractTypesByCountryId;
using Shared.Result;

namespace Core.Domains.ContractTypes;

public interface IContractTypeService
{
    public Task<Result<IEnumerable<GetContractTypesByCountryIdResponse>>> GetContractTypesByCountryIdAsync(long countryId, CancellationToken cancellationToken = default);
}