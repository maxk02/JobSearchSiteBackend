using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.RemoveCompanyBookmark;

public class RemoveCompanyBookmarkHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) : IRequestHandler<RemoveCompanyBookmarkRequest, Result>
{
    public async Task<Result> Handle(RemoveCompanyBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = jwtCurrentAccountService.GetIdOrThrow();

        if (currentAccountId != request.UserId)
            return Result.Forbidden();

        var user = await context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .FirstOrDefaultAsync(u => u.Id == currentAccountId, cancellationToken);

        if (user is null)
            return Result.Error();
        
        var company = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
        
        if (company is null)
            return Result.Error();
        
        user.BookmarkedCompanies?.Remove(company);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}