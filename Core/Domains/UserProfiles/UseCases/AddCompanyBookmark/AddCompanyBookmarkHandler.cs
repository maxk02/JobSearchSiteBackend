using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.UserProfiles.UseCases.AddCompanyBookmark;

public class AddCompanyBookmarkHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddCompanyBookmarkRequest, Result>
{
    public async Task<Result> Handle(AddCompanyBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var user = await context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .FirstOrDefaultAsync(u => u.Id == currentAccountId, cancellationToken);

        if (user is null)
            return Result.Error();
        
        var company = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
        
        if (company is null)
            return Result.Error();
        
        user.BookmarkedCompanies?.Add(company);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}