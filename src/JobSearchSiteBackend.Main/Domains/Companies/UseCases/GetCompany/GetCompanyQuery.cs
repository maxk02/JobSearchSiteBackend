using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;

public record GetCompanyQuery(long Id) : IRequest<Result<GetCompanyResult>>;