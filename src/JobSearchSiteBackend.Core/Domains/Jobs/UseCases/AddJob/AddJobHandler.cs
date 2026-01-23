using System.Globalization;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;

public class AddJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IInjectableSqlQueries sqlQueries) : IRequestHandler<AddJobCommand, Result<AddJobResult>>
{
    public async Task<Result<AddJobResult>> Handle(AddJobCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var dateTimePublishedUtc = DateTime.UtcNow;
        
        var job = new Job(
            command.CategoryId,
            command.CompanyId,
            command.Title,
            command.Description,
            command.IsPublic,
            dateTimePublishedUtc,
            command.DateTimeExpiringUtc,
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

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == command.CompanyId
                    && ucc.UserId == currentUserId
                    && ucc.ClaimId == CompanyClaim.CanEditJobs.Id)
                .AnyAsync();

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();

        
        var diff = command.DateTimeExpiringUtc.Subtract(dateTimePublishedUtc).TotalDays;
        var daysCeiled = (int)Math.Ceiling(diff);
        
        // suppose we have implemented it only for one currency for now
        var jobPublicationInterval = await context.JobPublicationIntervals
            .Include(jpi => jpi.CountryCurrency)
            .Where(jpi => jpi.CountryCurrency!.CountryId == command.CompanyId)
            .Where(jpi => jpi.MaxDaysOfPublication >= daysCeiled)
            .OrderBy(v => v.MaxDaysOfPublication)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (jobPublicationInterval is null)
            return Result.Error();

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        
        var currentBalance = await context.CompanyBalanceTransactions
            .FromSqlInterpolated(sqlQueries.GetCompanyBalanceTransactionsWithRowLocks(command.CompanyId))
            .SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0m;
        
        var newBalance = currentBalance + jobPublicationInterval.Price;

        if (newBalance < 0)
            return Result.Error();
        
        var companyBalanceTransaction = new CompanyBalanceTransaction(command.CompanyId, -jobPublicationInterval.Price,
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
        await transaction.CommitAsync(cancellationToken);

        var result = new AddJobResult(job.Id);
        
        return Result.Success(result);
    }
}