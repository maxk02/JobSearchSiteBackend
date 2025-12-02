using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddJobBookmark;

public class AddJobBookmarkHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobBookmarkCommand, Result>
{
    public async Task<Result> Handle(AddJobBookmarkCommand command, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var userJobBookmark = new UserJobBookmark(currentAccountId, command.JobId);
        
        context.UserJobBookmarks.Add(userJobBookmark);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}