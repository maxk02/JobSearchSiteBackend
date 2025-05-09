using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyById;

public record GetCompanyByIdRequest(long Id) : IRequest<Result<GetCompanyByIdResponse>>;