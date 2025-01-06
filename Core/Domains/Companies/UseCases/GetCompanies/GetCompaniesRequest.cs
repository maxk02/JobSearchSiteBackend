using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.Companies.UseCases.GetCompanies;

public record GetCompaniesRequest(long CountryId, string Query, PaginationSpec PaginationSpec)
    : IRequest<GetCompaniesResponse>;