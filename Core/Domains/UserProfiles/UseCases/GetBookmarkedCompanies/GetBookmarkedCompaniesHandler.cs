using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies;
using Core.Domains.Companies.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public class GetBookmarkedCompaniesHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) 
    : IRequestHandler<GetBookmarkedCompaniesRequest, Result<GetBookmarkedCompaniesResponse>>
{
    public async Task<Result<GetBookmarkedCompaniesResponse>> Handle(GetBookmarkedCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var query = context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.BookmarkedCompanies ?? new List<Company>());
        
        var count = await query.CountAsync(cancellationToken);
        
        var companyInfoDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ProjectTo<CompanyInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize, count);
        
        var response = new GetBookmarkedCompaniesResponse(companyInfoDtos, paginationResponse);

        return response;
    }
}