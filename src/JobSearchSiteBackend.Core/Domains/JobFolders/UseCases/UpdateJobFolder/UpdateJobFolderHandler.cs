using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;

public class UpdateJobFolderHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobFolderCommand, Result>
{
    public async Task<Result> Handle(UpdateJobFolderCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasEditInfoClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(command.Id, currentUserId,
                JobFolderClaim.CanEditInfo.Id)
            .AnyAsync(cancellationToken);

        if (!hasEditInfoClaim)
            return Result.Forbidden();
        
        var jobFolder = await context.JobFolders.FindAsync([command.Id], cancellationToken);
        if (jobFolder is null)
            return Result.NotFound();
        
        if (command.Name is not null)
            jobFolder.Name = command.Name;
        
        if (command.Description is not null)
            jobFolder.Description = command.Description;

        context.JobFolders.Update(jobFolder);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}