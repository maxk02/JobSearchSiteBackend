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

namespace Core.Domains.Jobs.UseCases.UpdateJob;

public class UpdateJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateJobRequest, Result>
{
    public async Task<Result> Handle(UpdateJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs.FindAsync([request.Id], cancellationToken);

        if (job is null)
            return Result.Error();

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderClosures
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();
        
        var jobSearchModel = new JobSearchModel(job.Id, job.RowVersion);

        if (request.JobFolderId is not null)
        {
            var hasPermissionInRequestedFolderOrAncestors =
                await context.JobFolderClosures
                    .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId.Value, currentUserId,
                        JobFolderClaim.CanEditJobsAndSubfolders.Id)
                    .AnyAsync(cancellationToken);

            if (!hasPermissionInRequestedFolderOrAncestors)
                return Result.Forbidden();

            job.JobFolderId = request.JobFolderId.Value;
            jobSearchModel.JobFolderId = request.JobFolderId.Value;
        }

        if (request.CategoryId is not null)
        {
            if (!Category.AllIds.Contains(request.CategoryId.Value))
                return Result.Error();

            job.CategoryId = request.CategoryId.Value;
            jobSearchModel.CategoryId = request.CategoryId.Value;
        }

        if (request.Title is not null)
        {
            job.Title = request.Title;
            jobSearchModel.Title = request.Title;
        }

        if (request.Description is not null)
        {
            job.Description = request.Description;
            jobSearchModel.Description = request.Description;
        }

        if (request.IsPublic is not null)
            job.IsPublic = request.IsPublic.Value;

        if (request.NewDateTimeExpiringUtc is not null)
        {
            var difference = request.NewDateTimeExpiringUtc.Value - DateTime.UtcNow;

            if (difference.Minutes < 1 || difference.Days > 30)
                return Result.Error();

            job.DateTimeExpiringUtc = request.NewDateTimeExpiringUtc.Value;
        }

        if (request.Responsibilities is not null)
        {
            job.Responsibilities = request.Responsibilities;
            jobSearchModel.Responsibilities = request.Responsibilities;
        }

        if (request.Requirements is not null)
        {
            job.Requirements = request.Requirements;
            jobSearchModel.Requirements = request.Requirements;
        }

        if (request.Advantages is not null)
        {
            job.Advantages = request.Advantages;
            jobSearchModel.Advantages = request.Advantages;
        }

        if (request.SalaryRecord is not null)
            job.SalaryRecord = request.SalaryRecord;

        if (request.EmploymentTypeRecord is not null)
            job.EmploymentTypeRecord = request.EmploymentTypeRecord;


        if (request.ContractTypeIds is not null)
        {
            var contractTypes = await context.ContractTypes
                .Where(jobContractType => request.ContractTypeIds.Contains(jobContractType.Id))
                .ToListAsync(cancellationToken);

            var nonExistentContractTypeIds =
                request.ContractTypeIds.Except(contractTypes.Select(contractType => contractType.Id));

            if (nonExistentContractTypeIds.Any())
                return Result.Error();

            job.JobContractTypes = contractTypes;
        }

        if (request.LocationIds is not null)
        {
            var locations = await context.Locations
                .Where(location => request.LocationIds.Contains(location.Id))
                .ToListAsync(cancellationToken);

            var nonExistentLocationIds =
                request.LocationIds.Except(locations.Select(location => location.Id));

            if (nonExistentLocationIds.Any())
                return Result.Error();

            job.Locations = locations;
        }

        var validator = new JobValidator();

        var validationResult = await validator.ValidateAsync(job, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Error();
        

        context.Jobs.Update(job);
        await context.SaveChangesAsync(cancellationToken);


        backgroundJobService.Enqueue(
            () => jobSearchRepository.UpdateIfNewestAsync(jobSearchModel, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );

        return Result.Success();
    }
}