using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies;
using Core.Domains.Companies.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public class GetBookmarkedCompaniesHandler(
    IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetBookmarkedCompaniesRequest, Result<GetBookmarkedCompaniesResponse>>
{
    public async Task<Result<GetBookmarkedCompaniesResponse>> Handle(GetBookmarkedCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = jwtCurrentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.UserId)
            return Result<GetBookmarkedCompaniesResponse>.Forbidden();

        var query = context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.BookmarkedCompanies ?? new List<Company>());
        
        var count = await query.CountAsync(cancellationToken);
        
        var bookmarkedCompanies = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);

        var companyInfocardDtos = bookmarkedCompanies
            .Select(x => new CompanyInfocardDto(x.Id, x.Name, x.CountryId)).ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);
        
        var response = new GetBookmarkedCompaniesResponse(companyInfocardDtos, paginationResponse);

        return response;
    }
}