using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.AddJob;

public class AddJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<AddJobRequest, Result>
{
    public async Task<Result> Handle(AddJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = new Job(request.CategoryId, request.JobFolderId, request.Title, request.Description, request.IsPublic,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), request.Responsibilities, request.Requirements,
            request.Advantages, request.SalaryRecord, request.EmploymentTypeRecord);

        var validator = new JobValidator();

        var validationResult = await validator.ValidateAsync(job, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Error();


        if (!Category.AllIds.Contains(request.CategoryId))
            return Result.Error();


        var jobFolder = await context.JobFolders
            .Include(jf => jf.Company)
            .Where(jf => jf.Id == request.JobFolderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobFolder is null)
            return Result.Error();

        var countryId = jobFolder.Company!.CountryId;
        var companyId = jobFolder.Company!.Id;

        var hasPermissionInRequestedFolderOrAncestors =
            await context.JobFolderClosures
                .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
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

        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        context.Jobs.Add(job);
        await context.SaveChangesAsync(cancellationToken);

        var jobSearchModel = new JobSearchModel(job.Id, job.RowVersion, job.JobFolderId,
            countryId, companyId, job.CategoryId, job.Title, job.Description,
            job.Responsibilities ?? [], job.Requirements ?? [], job.Advantages ?? []);

        await transaction.CommitAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => jobSearchRepository.AddOrSetConstFieldsAsync(jobSearchModel, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );

        return Result.Success();
    }
}