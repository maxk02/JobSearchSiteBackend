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

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;

public class AddJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<AddJobCommand, Result>
{
    public async Task<Result> Handle(AddJobCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var job = new Job(
            command.CategoryId,
            command.JobFolderId,
            command.Title,
            command.Description,
            command.IsPublic,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30),
            command.Responsibilities,
            command.Requirements,
            command.NiceToHaves,
            mapper.Map<JobSalaryInfo>(command.JobSalaryInfoDto),
            EmploymentOption.AllValues.Where(x => command.EmploymentTypeIds.Contains(x.Id)).ToList());

        var validator = new JobValidator();

        var validationResult = await validator.ValidateAsync(job, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Error();

        if (!Category.AllIds.Contains(command.CategoryId))
            return Result.Error();

        var jobFolder = await context.JobFolders
            .AsNoTracking()
            .Include(jobFolder => jobFolder.Company)
            .Where(jf => jf.Id == command.JobFolderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobFolder is null)
            return Result.Error();

        var countryId = jobFolder.Company!.CountryId;

        var hasPermissionInRequestedFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(command.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedFolderOrAncestors)
            return Result.Forbidden();


        var contractTypes = await context.ContractTypes
            .AsNoTracking()
            .Where(jobContractType => command.ContractTypeIds.Contains(jobContractType.Id))
            .ToListAsync(cancellationToken);

        var nonExistentContractTypeIds =
            command.ContractTypeIds.Except(contractTypes.Select(contractType => contractType.Id));

        if (nonExistentContractTypeIds.Any())
            return Result.Error();

        job.JobContractTypes = contractTypes;


        var locations = await context.Locations
            .AsNoTracking()
            .Where(location => command.LocationIds.Contains(location.Id))
            .ToListAsync(cancellationToken);

        var nonExistentLocationIds =
            command.LocationIds.Except(locations.Select(location => location.Id));

        if (nonExistentLocationIds.Any())
            return Result.Error();

        job.Locations = locations;

        context.Jobs.Add(job);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}