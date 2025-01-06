using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies;
using Core.Domains.UserProfiles.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public class GetBookmarkedCompaniesHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetBookmarkedCompaniesRequest, Result<GetBookmarkedCompaniesResponse>>
{
    public async Task<Result<GetBookmarkedCompaniesResponse>> Handle(GetBookmarkedCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
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

        var companyShortDtos = bookmarkedCompanies
            .Select(x => new CompanyShortDto(x.Id, x.Name, x.Description, x.CountryId)).ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);
        
        var response = new GetBookmarkedCompaniesResponse(companyShortDtos, paginationResponse);

        return response;
    }
}