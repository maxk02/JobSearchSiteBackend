using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobContractTypes.UseCases.GetJobContractTypesByCountryId;

public class GetJobContractTypesByCountryIdHandler(IJobContractTypeRepository jobContractTypeRepository)
    : IRequestHandler<GetJobContractTypesByCountryIdRequest, Result<ICollection<GetJobContractTypesByCountryIdResponse>>>
{
    public async Task<Result<ICollection<GetJobContractTypesByCountryIdResponse>>> Handle(
        GetJobContractTypesByCountryIdRequest request, CancellationToken cancellationToken = default)
    {
        var contractTypes = await jobContractTypeRepository
            .GetByCountryIdAsync(request.CountryId, cancellationToken);

        return Result<ICollection<GetJobContractTypesByCountryIdResponse>>
            .Success(contractTypes.Select(ct => new GetJobContractTypesByCountryIdResponse(ct.CountryId, ct.Name)).ToList());
    }
}