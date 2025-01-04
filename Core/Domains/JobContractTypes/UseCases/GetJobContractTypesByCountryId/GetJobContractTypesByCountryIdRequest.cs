using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobContractTypes.UseCases.GetJobContractTypesByCountryId;

public record GetJobContractTypesByCountryIdRequest(long CountryId)
    : IRequest<Result<GetJobContractTypesByCountryIdResponse>>;