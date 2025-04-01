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

        if (request.JobFolderId is not null)
        {
            var newCompanyId = await context.JobFolders
                .Where(jf => jf.Id == request.JobFolderId)
                .Select(jf => jf.CompanyId)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (newCompanyId != companyId)
                return Result.Error();
            
            var hasPermissionInRequestedFolderOrAncestors =
                await context.JobFolderRelations
                    .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId.Value, currentUserId,
                        JobFolderClaim.CanEditJobsAndSubfolders.Id)
                    .AnyAsync(cancellationToken);

            if (!hasPermissionInRequestedFolderOrAncestors)
                return Result.Forbidden();

            job.JobFolderId = request.JobFolderId.Value;
        }

        if (request.CategoryId is not null)
        {
            if (!Category.AllIds.Contains(request.CategoryId.Value))
                return Result.Error();

            job.CategoryId = request.CategoryId.Value;
        }

        if (request.Title is not null)
            job.Title = request.Title;

        if (request.Description is not null)
            job.Description = request.Description;

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
            job.Responsibilities = request.Responsibilities;

        if (request.Requirements is not null)
            job.Requirements = request.Requirements;

        if (request.Advantages is not null)
            job.NiceToHaves = request.Advantages;

        if (request.SalaryInfo is not null)
            job.SalaryInfo = mapper.Map<JobSalaryInfo>(request.SalaryInfo);
        
        if (request.EmploymentTypeIds is not null)
        {
            var employmentTypes = EmploymentType.AllValues
                .Where(employmentType => request.EmploymentTypeIds.Contains(employmentType.Id))
                .ToList();

            var nonExistentEmploymentTypeIds =
                request.EmploymentTypeIds.Except(employmentTypes.Select(employmentType => employmentType.Id));

            if (nonExistentEmploymentTypeIds.Any())
                return Result.Error();

            job.EmploymentTypes = employmentTypes;
        }

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