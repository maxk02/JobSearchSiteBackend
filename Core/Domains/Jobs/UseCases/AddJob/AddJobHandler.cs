using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using Core.Domains.EmploymentOptions;

namespace Core.Domains.Jobs.UseCases.AddJob;

public class AddJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<AddJobRequest, Result>
{
    public async Task<Result> Handle(AddJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var job = new Job(
            request.CategoryId,
            request.JobFolderId,
            request.Title,
            request.Description,
            request.IsPublic,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30),
            request.Responsibilities,
            request.Requirements,
            request.NiceToHaves,
            mapper.Map<JobSalaryInfo>(request.JobSalaryInfoDto),
            EmploymentOption.AllValues.Where(x => request.EmploymentTypeIds.Contains(x.Id)).ToList());

        var validator = new JobValidator();

        var validationResult = await validator.ValidateAsync(job, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Error();

        if (!Category.AllIds.Contains(request.CategoryId))
            return Result.Error();

        var jobFolder = await context.JobFolders
            .AsNoTracking()
            .Include(jobFolder => jobFolder.Company)
            .Where(jf => jf.Id == request.JobFolderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobFolder is null)
            return Result.Error();

        var countryId = jobFolder.Company!.CountryId;

        var hasPermissionInRequestedFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedFolderOrAncestors)
            return Result.Forbidden();


        var contractTypes = await context.ContractTypes
            .AsNoTracking()
            .Where(jobContractType => request.ContractTypeIds.Contains(jobContractType.Id))
            .ToListAsync(cancellationToken);

        var nonExistentContractTypeIds =
            request.ContractTypeIds.Except(contractTypes.Select(contractType => contractType.Id));

        if (nonExistentContractTypeIds.Any())
            return Result.Error();

        job.JobContractTypes = contractTypes;


        var locations = await context.Locations
            .AsNoTracking()
            .Where(location => request.LocationIds.Contains(location.Id))
            .ToListAsync(cancellationToken);

        var nonExistentLocationIds =
            request.LocationIds.Except(locations.Select(location => location.Id));

        if (nonExistentLocationIds.Any())
            return Result.Error();

        job.Locations = locations;

        context.Jobs.Add(job);

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await context.SaveChangesAsync(cancellationToken);

        var jobSearchModel = new JobSearchModel(
            job.Id,
            countryId,
            job.CategoryId,
            job.Title,
            job.Description,
            job.Responsibilities ?? [],
            job.Requirements ?? [],
            job.NiceToHaves ?? []
        );

        backgroundJobService.Enqueue(
            () => jobSearchRepository
                .AddOrUpdateIfNewestAsync(jobSearchModel, job.RowVersion, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );

        transaction.Complete();

        return Result.Success();
    }
}