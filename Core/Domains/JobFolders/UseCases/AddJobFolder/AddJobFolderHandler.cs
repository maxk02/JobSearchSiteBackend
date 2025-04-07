using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.JobFolders.UseCases.AddJobFolder;

public class AddJobFolderHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobFolderRequest, Result<long>>
{
    public async Task<Result<long>> Handle(AddJobFolderRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var currentUserClaimIdsForParentAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(request.ParentId, currentUserId)
            .ToListAsync(cancellationToken);

        if (!currentUserClaimIdsForParentAndAncestors.Contains(JobFolderClaim.CanEditJobs.Id))
            return Result<long>.Forbidden();

        var parentFolderCompanyId = await context.JobFolders
            .Where(jf => jf.Id == request.ParentId)
            .Select(jf => jf.CompanyId)
            .SingleOrDefaultAsync(cancellationToken);

        if (parentFolderCompanyId != request.CompanyId)
            return Result<long>.Error();

        var jobFolder = new JobFolder(request.CompanyId, request.Name, request.Description);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        context.JobFolders.Add(jobFolder);
        await context.SaveChangesAsync(cancellationToken);

        var parentClosures = await context.JobFolderRelations
            .Where(c => c.DescendantId == request.ParentId)
            .ToListAsync(cancellationToken);

        var newClosures = parentClosures
            .Select(c => new JobFolderRelation(c.AncestorId, jobFolder.Id, c.Depth + 1))
            .Append(new JobFolderRelation(jobFolder.Id, jobFolder.Id, 0));

        context.JobFolderRelations.AddRange(newClosures);

        context.UserJobFolderClaims.AddRange(
            JobFolderClaim.AllIds.Select(id => new UserJobFolderClaim(currentUserId, jobFolder.Id, id))
        );

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        
        return jobFolder.Id;
    }
}