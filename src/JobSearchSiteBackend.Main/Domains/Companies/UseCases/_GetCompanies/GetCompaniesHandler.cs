// using Ardalis.Result;
// using AutoMapper;
// using AutoMapper.QueryableExtensions;
// using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
// using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
// using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
// using JobSearchSiteBackend.Core.Domains.Companies.Search;
// using JobSearchSiteBackend.Core.Persistence;
// using Microsoft.EntityFrameworkCore;
//
// namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanies;
//
// public class GetCompaniesHandler(
//     ICompanySearchRepository companySearchRepository,
//     MainDataContext context,
//     IMapper mapper) : IRequestHandler<GetCompaniesRequest, Result<GetCompaniesResponse>>
// {
//     public async Task<Result<GetCompaniesResponse>> Handle(GetCompaniesRequest request,
//         CancellationToken cancellationToken = default)
//     {
//         var hitIds = await companySearchRepository.SearchFromCountryIdAsync(request.CountryId, request.Query, cancellationToken);
//
//         var query = context.Companies
//             .Where(c => c.CountryId == request.CountryId
//                         && hitIds.Contains(c.Id)
//                         && c.IsPublic == true);
//
//         var count = await query.CountAsync(cancellationToken);
//
//         var companyInfoDtos = await query
//             .Skip((request.Page - 1) * request.Size)
//             .Take(request.Size)
//             .ProjectTo<CompanyDto>(mapper.ConfigurationProvider)
//             .ToListAsync(cancellationToken);
//
//         var paginationResponse = new PaginationResponse(request.Page,
//             request.Size, count);
//
//         var response = new GetCompaniesResponse(companyInfoDtos, paginationResponse);
//
//         return response;
//     }
// }