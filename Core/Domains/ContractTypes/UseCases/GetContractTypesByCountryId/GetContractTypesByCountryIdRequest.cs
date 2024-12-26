using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.ContractTypes.UseCases.GetContractTypesByCountryId;

public record GetContractTypesByCountryIdRequest(long CountryId)
    : IRequest<Result<ICollection<GetContractTypesByCountryIdResponse>>>;