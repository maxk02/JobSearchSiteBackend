using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

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
        
        var hasEditClaim = await context.JobFolderClosures
            .GetThisOrAncestorWhereUserHasClaim(request.Id, currentUserId,
                JobFolderClaim.CanReadJobsAndSubfolders.Id)
            .AnyAsync(cancellationToken);

        if (!hasEditClaim)
            return Result.Forbidden();
        
        context.JobFolders.Remove(jobFolder);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}