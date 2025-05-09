using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddJobBookmark;

public class AddJobBookmarkHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobBookmarkRequest, Result>
{
    public async Task<Result> Handle(AddJobBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var user = await context.UserProfiles
            .Include(u => u.BookmarkedCompanies)
            .FirstOrDefaultAsync(u => u.Id == currentAccountId, cancellationToken);

        if (user is null)
            return Result.Error();
        
        var job = await context.Jobs
            .FirstOrDefaultAsync(c => c.Id == request.JobId, cancellationToken);
        
        if (job is null)
            return Result.Error();
        
        user.BookmarkedJobs?.Add(job);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}