using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.DeleteJobFolder;

public class DeleteJobFolderHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<DeleteJobFolderCommand, Result>
{
    public async Task<Result> Handle(DeleteJobFolderCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var jobFolder = await context.JobFolders.FindAsync([command.Id], cancellationToken);
        if (jobFolder is null)
            return Result.NotFound();
        
        var hasEditClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(command.Id, currentUserId,
                JobFolderClaim.CanEditJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasEditClaim)
            return Result.Forbidden();
        
        var thisAndDescendants = await context.JobFolderRelations
            .Where(jfr => jfr.AncestorId == command.Id)
            .Select(jfr => new { Relation = jfr, JobFolder = jfr.Descendant! })
            .ToListAsync(cancellationToken);
        
        context.JobFolderRelations.RemoveRange(thisAndDescendants.Select(x => x.Relation));
        context.JobFolders.RemoveRange(thisAndDescendants.Select(x => x.JobFolder));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}