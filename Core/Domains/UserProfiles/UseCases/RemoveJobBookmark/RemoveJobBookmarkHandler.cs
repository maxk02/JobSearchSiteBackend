using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.RemoveJobBookmark;

public class RemoveJobBookmarkHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) : IRequestHandler<RemoveJobBookmarkRequest, Result>
{
    public async Task<Result> Handle(RemoveJobBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = jwtCurrentAccountService.GetIdOrThrow();

        if (currentAccountId != request.UserId)
            return Result.Forbidden();

        var user = await context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            return Result.Error();
        
        var job = await context.Jobs
            .FirstOrDefaultAsync(c => c.Id == request.JobId, cancellationToken);
        
        if (job is null)
            return Result.Error();
        
        user.BookmarkedJobs?.Remove(job);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}