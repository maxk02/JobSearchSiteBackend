using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.ContractTypes.UseCases.GetContractTypesByCountryId;

public class GetContractTypesByCountryIdHandler(IContractTypeRepository contractTypeRepository)
    : IRequestHandler<GetContractTypesByCountryIdRequest, Result<ICollection<GetContractTypesByCountryIdResponse>>>
{
    public async Task<Result<ICollection<GetContractTypesByCountryIdResponse>>> Handle(
        GetContractTypesByCountryIdRequest request, CancellationToken cancellationToken = default)
    {
        var contractTypes = await contractTypeRepository
            .GetByCountryIdAsync(request.CountryId, cancellationToken);

        return Result<ICollection<GetContractTypesByCountryIdResponse>>
            .Success(contractTypes.Select(ct => new GetContractTypesByCountryIdResponse(ct.CountryId, ct.Name)).ToList());
    }
}