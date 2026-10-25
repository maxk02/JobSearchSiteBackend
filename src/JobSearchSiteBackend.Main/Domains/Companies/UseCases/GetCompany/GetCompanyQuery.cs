using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompany;

public record GetCompanyQuery(long Id) : IRequest<Result<GetCompanyResult>>;