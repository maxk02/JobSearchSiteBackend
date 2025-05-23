﻿using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;

public class DeleteJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<DeleteJobRequest, Result>
{
    public async Task<Result> Handle(DeleteJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs
            .Include(job => job.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .Where(job => job.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result.Error();

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();

        var countryId = job.JobFolder!.Company!.CountryId;
        var jobRowVersion = job.RowVersion;
        
        var jobSearchModel = new JobSearchModel(
            job.Id,
            countryId,
            job.CategoryId,
            job.Title,
            job.Description,
            job.Responsibilities!,
            job.Requirements!,
            job.NiceToHaves!
        );
        
        context.Jobs.Remove(job);
        
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => jobSearchRepository.SoftDeleteAsync(jobSearchModel, jobRowVersion, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );
        
        transaction.Complete();

        return Result.Success();
    }
}