using Core.Domains.ContractTypes.UseCases.GetContractTypesByCountryId;
using Shared.Result;

namespace Core.Domains.ContractTypes;

public class ContractTypeService(IContractTypeRepository contractTypeRepository) : IContractTypeService
{
    public async Task<Result<IEnumerable<GetContractTypesByCountryIdResponse>>> GetContractTypesByCountryIdAsync(
        long countryId, CancellationToken cancellationToken = default)
    {
        var contractTypes = await contractTypeRepository.GetByCountryIdAsync(countryId, cancellationToken);

        return Result<IEnumerable<GetContractTypesByCountryIdResponse>>
            .Success(contractTypes.Select(ct => new GetContractTypesByCountryIdResponse(ct.CountryId, ct.Name)));
    }
}