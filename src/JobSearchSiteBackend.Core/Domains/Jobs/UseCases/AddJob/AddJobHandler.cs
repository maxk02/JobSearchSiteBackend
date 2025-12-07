using System.Globalization;
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;

public class AddJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddJobCommand, Result<AddJobResult>>
{
    public async Task<Result<AddJobResult>> Handle(AddJobCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var dateTimePublishedUtc = DateTime.UtcNow;
        
        var job = new Job(
            command.CategoryId,
            command.JobFolderId,
            command.Title,
            command.Description,
            command.IsPublic,
            dateTimePublishedUtc,
            command.DateTimeExpiringUtc,
            command.Responsibilities,
            command.Requirements,
            command.NiceToHaves,
            command.JobSalaryInfoDto?.ToJobSalaryInfo(),
            EmploymentOption.AllValues.Where(x => command.EmploymentOptionIds.Contains(x.Id)).ToList());

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

        var hasPermissionInRequestedFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(command.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedFolderOrAncestors)
            return Result.Forbidden();

        
        var diff = command.DateTimeExpiringUtc.Subtract(dateTimePublishedUtc).TotalDays;
        var daysCeiled = (int)Math.Ceiling(diff);
        
        // suppose we have implemented it only for one currency for now
        var jobPublicationInterval = await context.JobPublicationIntervals
            .Include(jpi => jpi.CountryCurrency)
            .Where(jpi => jpi.CountryCurrency!.CountryId == jobFolder.CompanyId)
            .Where(jpi => jpi.MaxDaysOfPublication >= daysCeiled)
            .OrderBy(v => v.MaxDaysOfPublication)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (jobPublicationInterval is null)
            return Result.Error();

        var companyBalanceTransaction = new CompanyBalanceTransaction(jobFolder.CompanyId, -jobPublicationInterval.Price,
            $"Publikacja ogłoszenia {command.Title} do {command.DateTimeExpiringUtc.ToString(CultureInfo.InvariantCulture)}",
            jobPublicationInterval.CountryCurrency!.CurrencyId, currentUserId);
        
        context.CompanyBalanceTransactions.Add(companyBalanceTransaction);
        

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

        var result = new AddJobResult(job.Id);
        
        return Result.Success(result);
    }
}