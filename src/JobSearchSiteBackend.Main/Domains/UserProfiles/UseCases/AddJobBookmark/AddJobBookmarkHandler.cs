using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Persistence;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddJobBookmark;

public class AddJobBookmarkHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobBookmarkCommand, Result>
{
    public async Task<Result> Handle(AddJobBookmarkCommand command, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var bookmarkExists = await context.UserJobBookmarks
            .AnyAsync(ujb => ujb.JobId == command.JobId && ujb.UserId == currentAccountId, cancellationToken);

        if (bookmarkExists)
            return Result.Error();

        var userJobBookmark = new UserJobBookmark(currentAccountId, command.JobId);
        
        context.UserJobBookmarks.Add(userJobBookmark);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}