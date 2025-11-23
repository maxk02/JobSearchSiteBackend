using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.AddJobFolder;

public class AddJobFolderHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobFolderCommand, Result<long>>
{
    public async Task<Result<long>> Handle(AddJobFolderCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var currentUserClaimIdsForParentAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(command.ParentId, currentUserId)
            .ToListAsync(cancellationToken);

        if (!currentUserClaimIdsForParentAndAncestors.Contains(JobFolderClaim.CanEditJobs.Id))
            return Result<long>.Forbidden();

        var parentFolderCompanyId = await context.JobFolders
            .Where(jf => jf.Id == command.ParentId)
            .Select(jf => jf.CompanyId)
            .SingleOrDefaultAsync(cancellationToken);

        if (parentFolderCompanyId != command.CompanyId)
            return Result<long>.Error();

        var jobFolder = new JobFolder(command.CompanyId, command.Name, command.Description);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        context.JobFolders.Add(jobFolder);
        await context.SaveChangesAsync(cancellationToken);

        var parentClosures = await context.JobFolderRelations
            .Where(c => c.DescendantId == command.ParentId)
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