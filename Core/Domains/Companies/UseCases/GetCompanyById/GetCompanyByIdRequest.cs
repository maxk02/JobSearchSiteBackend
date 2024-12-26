using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.GetCompanyById;

public record GetCompanyByIdRequest(long Id) : IRequest<Result<GetCompanyByIdResponse>>;