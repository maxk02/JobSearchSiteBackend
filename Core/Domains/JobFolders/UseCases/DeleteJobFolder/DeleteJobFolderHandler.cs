using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.DeleteJobFolder;

public class DeleteJobFolderHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<DeleteJobFolderRequest, Result>
{
    public async Task<Result> Handle(DeleteJobFolderRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var jobFolder = await context.JobFolders.FindAsync([request.Id], cancellationToken);
        if (jobFolder is null)
            return Result.NotFound();
        
        var hasEditClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.Id, currentUserId,
                JobFolderClaim.CanEditJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasEditClaim)
            return Result.Forbidden();
        
        var thisAndDescendants = await context.JobFolderRelations
            .Where(jfr => jfr.AncestorId == request.Id)
            .Select(jfr => new { Relation = jfr, JobFolder = jfr.Descendant! })
            .ToListAsync(cancellationToken);
        
        context.JobFolderRelations.RemoveRange(thisAndDescendants.Select(x => x.Relation));
        context.JobFolders.RemoveRange(thisAndDescendants.Select(x => x.JobFolder));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}