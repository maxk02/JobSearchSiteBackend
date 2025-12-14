using System.Globalization;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;

public class UpdateJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobCommand, Result>
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
        
        var companyId = job.JobFolder!.CompanyId;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();

        if (command.NewDateTimeExpiringUtc is not null)
        {
            var oldDiff = job.DateTimeExpiringUtc.Subtract(job.DateTimePublishedUtc).TotalDays;
            var oldDaysCeiled = (int)Math.Ceiling(oldDiff);
            
            var newDiff = command.NewDateTimeExpiringUtc.Value.Subtract(job.DateTimePublishedUtc).TotalDays;
            var newDaysCeiled = (int)Math.Ceiling(newDiff);
            
            var oldJobPublicationInterval = await context.JobPublicationIntervals
                .Include(jpi => jpi.CountryCurrency)
                .Where(jpi => jpi.CountryCurrency!.CountryId == job.JobFolder.CompanyId)
                .Where(jpi => jpi.MaxDaysOfPublication >= oldDaysCeiled)
                .OrderBy(v => v.MaxDaysOfPublication)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (oldJobPublicationInterval is null)
                return Result.Error();
            
            // suppose we have implemented it only for one currency for now
            var newJobPublicationInterval = await context.JobPublicationIntervals
                .Include(jpi => jpi.CountryCurrency)
                .Where(jpi => jpi.CountryCurrency!.CountryId == job.JobFolder.CompanyId)
                .Where(jpi => jpi.MaxDaysOfPublication >= newDaysCeiled)
                .OrderBy(v => v.MaxDaysOfPublication)
                .FirstOrDefaultAsync(cancellationToken);
        
            if (newJobPublicationInterval is null)
                return Result.Error();
            
            if (oldJobPublicationInterval.CountryCurrency!.CurrencyId 
                != newJobPublicationInterval.CountryCurrency!.CurrencyId)
                return Result.Error();

            var priceDifference = newJobPublicationInterval.Price - oldJobPublicationInterval.Price;

            if (priceDifference > 0)
            {
                var companyBalanceTransaction = new CompanyBalanceTransaction(job.JobFolder.CompanyId, -priceDifference,
                    $"Aktualizacja terminu wygaśnięcia ogłoszenia {job.Id} do " +
                    $"{command.NewDateTimeExpiringUtc.Value.ToString(CultureInfo.InvariantCulture)}",
                    newJobPublicationInterval.CountryCurrency!.CurrencyId, currentUserId);
        
                context.CompanyBalanceTransactions.Add(companyBalanceTransaction);
            }
        }

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

        if (command.NiceToHaves is not null)
            job.NiceToHaves = command.NiceToHaves;

        if (command.SalaryInfo is not null)
            job.SalaryInfo = command.SalaryInfo.ToJobSalaryInfo(command.Id);
        
        if (command.EmploymentOptionIds is not null)
        {
            var employmentTypes = EmploymentOption.AllValues
                .Where(employmentType => command.EmploymentOptionIds.Contains(employmentType.Id))
                .ToList();

            var nonExistentEmploymentTypeIds =
                command.EmploymentOptionIds.Except(employmentTypes.Select(employmentType => employmentType.Id));

            if (nonExistentEmploymentTypeIds.Any())
                return Result.Error();

            job.EmploymentOptions = employmentTypes;
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
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}