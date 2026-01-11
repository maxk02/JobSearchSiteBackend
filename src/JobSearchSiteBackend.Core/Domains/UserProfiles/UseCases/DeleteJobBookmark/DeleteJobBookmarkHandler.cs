using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;

public class DeleteJobBookmarkHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<DeleteJobBookmarkCommand, Result>
{
    public async Task<Result> Handle(DeleteJobBookmarkCommand command, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var userJobBookmark = await context.UserJobBookmarks
            .Where(ujb => ujb.JobId == command.JobId && ujb.UserId == currentAccountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userJobBookmark is null)
            return Result.Error();
        
        context.UserJobBookmarks.Remove(userJobBookmark);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}