using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.JobFolders.UseCases.UpdateJobFolder;

public class UpdateJobFolderHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobFolderRequest, Result>
{
    public async Task<Result> Handle(UpdateJobFolderRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasEditInfoClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.Id, currentUserId,
                JobFolderClaim.CanEditInfo.Id)
            .AnyAsync(cancellationToken);

        if (!hasEditInfoClaim)
            return Result.Forbidden();
        
        var jobFolder = await context.JobFolders.FindAsync([request.Id], cancellationToken);
        if (jobFolder is null)
            return Result.NotFound();
        
        if (request.Name is not null)
            jobFolder.Name = request.Name;
        
        if (request.Description is not null)
            jobFolder.Description = request.Description;

        context.JobFolders.Update(jobFolder);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}