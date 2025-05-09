using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanies;

public record GetCompaniesRequest(long CountryId, string Query, PaginationSpec PaginationSpec)
    : IRequest<Result<GetCompaniesResponse>>;