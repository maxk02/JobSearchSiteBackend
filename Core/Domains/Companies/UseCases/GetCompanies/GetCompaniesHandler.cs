using Ardalis.Result;
using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Dtos;
using Core.Domains.Companies.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Companies.UseCases.GetCompanies;

public class GetCompaniesHandler(
    ICompanySearchRepository companySearchRepository,
    MainDataContext context) : IRequestHandler<GetCompaniesRequest, Result<GetCompaniesResponse>>
{
    public async Task<Result<GetCompaniesResponse>> Handle(GetCompaniesRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await companySearchRepository.SearchFromCountryIdAsync(request.CountryId, request.Query, cancellationToken);

        var query = context.Companies
            .Where(c => c.CountryId == request.CountryId
                        && hitIds.Contains(c.Id)
                        && c.IsPublic == true);

        var count = await query.CountAsync(cancellationToken);

        var companies = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);

        var companyInfocardDtos = companies
            .Select(x => new CompanyInfocardDto(x.Id, x.Name, x.CountryId)).ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetCompaniesResponse(companyInfocardDtos, paginationResponse);

        return response;
    }
}