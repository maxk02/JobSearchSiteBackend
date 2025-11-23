using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;

public class UpdateJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<UpdateJobCommand, Result>
{
    public async Task<Result> Handle(UpdateJobCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs
            .Include(job => job.JobFolder)
            .ThenInclude(jobFolder => jobFolder!.Company)
            .Where(job => job.Id == command.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result.Error();

        var countryId = job.JobFolder!.Company!.CountryId;
        var companyId = job.JobFolder!.CompanyId;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();

        if (command.JobFolderId is not null)
        {
            var newCompanyId = await context.JobFolders
                .Where(jf => jf.Id == command.JobFolderId)
                .Select(jf => jf.CompanyId)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (newCompanyId != companyId)
                return Result.Error();
            
            var hasPermissionInRequestedFolderOrAncestors =
                await context.JobFolderRelations
                    .GetThisOrAncestorWhereUserHasClaim(command.JobFolderId.Value, currentUserId,
                        JobFolderClaim.CanEditJobs.Id)
                    .AnyAsync(cancellationToken);

            if (!hasPermissionInRequestedFolderOrAncestors)
                return Result.Forbidden();

            job.JobFolderId = command.JobFolderId.Value;
        }

        if (command.CategoryId is not null)
        {
            if (!Category.AllIds.Contains(command.CategoryId.Value))
                return Result.Error();

            job.CategoryId = command.CategoryId.Value;
        }

        if (command.Title is not null)
            job.Title = command.Title;

        if (command.Description is not null)
            job.Description = command.Description;

        if (command.IsPublic is not null)
            job.IsPublic = command.IsPublic.Value;

        if (command.NewDateTimeExpiringUtc is not null)
        {
            var difference = command.NewDateTimeExpiringUtc.Value - DateTime.UtcNow;

            if (difference.Minutes < 1 || difference.Days > 30)
                return Result.Error();

            job.DateTimeExpiringUtc = command.NewDateTimeExpiringUtc.Value;
        }

        if (command.Responsibilities is not null)
            job.Responsibilities = command.Responsibilities;

        if (command.Requirements is not null)
            job.Requirements = command.Requirements;

        if (command.Advantages is not null)
            job.NiceToHaves = command.Advantages;

        if (command.SalaryInfo is not null)
            job.SalaryInfo = mapper.Map<JobSalaryInfo>(command.SalaryInfo);
        
        if (command.EmploymentTypeIds is not null)
        {
            var employmentTypes = EmploymentOption.AllValues
                .Where(employmentType => command.EmploymentTypeIds.Contains(employmentType.Id))
                .ToList();

            var nonExistentEmploymentTypeIds =
                command.EmploymentTypeIds.Except(employmentTypes.Select(employmentType => employmentType.Id));

            if (nonExistentEmploymentTypeIds.Any())
                return Result.Error();

            job.EmploymentTypes = employmentTypes;
        }

        if (command.ContractTypeIds is not null)
        {
            var contractTypes = await context.ContractTypes
                .Where(jobContractType => command.ContractTypeIds.Contains(jobContractType.Id))
                .ToListAsync(cancellationToken);

            var nonExistentContractTypeIds =
                command.ContractTypeIds.Except(contractTypes.Select(contractType => contractType.Id));

            if (nonExistentContractTypeIds.Any())
                return Result.Error();

            job.JobContractTypes = contractTypes;
        }

        if (command.LocationIds is not null)
        {
            var locations = await context.Locations
                .Where(location => command.LocationIds.Contains(location.Id))
                .ToListAsync(cancellationToken);

            var nonExistentLocationIds =
                command.LocationIds.Except(locations.Select(location => location.Id));

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

        return Result.Success();
    }
}