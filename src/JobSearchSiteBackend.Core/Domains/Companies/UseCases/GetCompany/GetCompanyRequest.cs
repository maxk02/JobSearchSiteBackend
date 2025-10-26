using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;

public record GetCompanyRequest(long Id) : IRequest<Result<GetCompanyResponse>>;