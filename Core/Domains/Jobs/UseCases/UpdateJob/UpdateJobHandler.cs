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
using Core.Domains.EmploymentTypes;

namespace Core.Domains.Jobs.UseCases.UpdateJob;

public class UpdateJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<UpdateJobRequest, Result>
{
    public async Task<Result> Handle(UpdateJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs
            .Include(job => job.JobFolder)
            .ThenInclude(jobFolder => jobFolder!.Company)
            .Where(job => job.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result.Error();

        var countryId = job.JobFolder!.Company!.CountryId;
        var companyId = job.JobFolder!.CompanyId;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();
        
        var jobUpdateDto = request.Job;

        if (jobUpdateDto.JobFolderId is not null)
        {
            var newCompanyId = await context.JobFolders
                .Where(jf => jf.Id == jobUpdateDto.JobFolderId)
                .Select(jf => jf.CompanyId)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (newCompanyId != companyId)
                return Result.Error();
            
            var hasPermissionInRequestedFolderOrAncestors =
                await context.JobFolderRelations
                    .GetThisOrAncestorWhereUserHasClaim(jobUpdateDto.JobFolderId.Value, currentUserId,
                        JobFolderClaim.CanEditJobsAndSubfolders.Id)
                    .AnyAsync(cancellationToken);

            if (!hasPermissionInRequestedFolderOrAncestors)
                return Result.Forbidden();

            job.JobFolderId = jobUpdateDto.JobFolderId.Value;
        }

        if (jobUpdateDto.CategoryId is not null)
        {
            if (!Category.AllIds.Contains(jobUpdateDto.CategoryId.Value))
                return Result.Error();

            job.CategoryId = jobUpdateDto.CategoryId.Value;
        }

        if (jobUpdateDto.Title is not null)
            job.Title = jobUpdateDto.Title;

        if (jobUpdateDto.Description is not null)
            job.Description = jobUpdateDto.Description;

        if (jobUpdateDto.IsPublic is not null)
            job.IsPublic = jobUpdateDto.IsPublic.Value;

        if (jobUpdateDto.NewDateTimeExpiringUtc is not null)
        {
            var difference = jobUpdateDto.NewDateTimeExpiringUtc.Value - DateTime.UtcNow;

            if (difference.Minutes < 1 || difference.Days > 30)
                return Result.Error();

            job.DateTimeExpiringUtc = jobUpdateDto.NewDateTimeExpiringUtc.Value;
        }

        if (jobUpdateDto.Responsibilities is not null)
            job.Responsibilities = jobUpdateDto.Responsibilities;

        if (jobUpdateDto.Requirements is not null)
            job.Requirements = jobUpdateDto.Requirements;

        if (jobUpdateDto.Advantages is not null)
            job.NiceToHaves = jobUpdateDto.Advantages;

        if (jobUpdateDto.SalaryInfo is not null)
            job.SalaryInfo = mapper.Map<JobSalaryInfo>(jobUpdateDto.SalaryInfo);
        
        if (jobUpdateDto.EmploymentTypeIds is not null)
        {
            var employmentTypes = EmploymentType.AllValues
                .Where(employmentType => jobUpdateDto.EmploymentTypeIds.Contains(employmentType.Id))
                .ToList();

            var nonExistentEmploymentTypeIds =
                jobUpdateDto.EmploymentTypeIds.Except(employmentTypes.Select(employmentType => employmentType.Id));

            if (nonExistentEmploymentTypeIds.Any())
                return Result.Error();

            job.EmploymentTypes = employmentTypes;
        }

        if (jobUpdateDto.ContractTypeIds is not null)
        {
            var contractTypes = await context.ContractTypes
                .Where(jobContractType => jobUpdateDto.ContractTypeIds.Contains(jobContractType.Id))
                .ToListAsync(cancellationToken);

            var nonExistentContractTypeIds =
                jobUpdateDto.ContractTypeIds.Except(contractTypes.Select(contractType => contractType.Id));

            if (nonExistentContractTypeIds.Any())
                return Result.Error();

            job.JobContractTypes = contractTypes;
        }

        if (jobUpdateDto.LocationIds is not null)
        {
            var locations = await context.Locations
                .Where(location => jobUpdateDto.LocationIds.Contains(location.Id))
                .ToListAsync(cancellationToken);

            var nonExistentLocationIds =
                jobUpdateDto.LocationIds.Except(locations.Select(location => location.Id));

            if (nonExistentLocationIds.Any())
                return Result.Error();

            job.Locations = locations;
        }

        var validator = new JobValidator();

        var validationResult = await validator.ValidateAsync(job, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Error();
        
        context.Jobs.Update(job);
        
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await context.SaveChangesAsync(cancellationToken);

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

        backgroundJobService.Enqueue(
            () => jobSearchRepository
                .AddOrUpdateIfNewestAsync(jobSearchModel, job.RowVersion, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );
        
        transaction.Complete();

        return Result.Success();
    }
}