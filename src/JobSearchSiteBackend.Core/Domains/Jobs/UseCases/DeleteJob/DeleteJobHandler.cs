using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;

public class DeleteJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<DeleteJobCommand, Result>
{
    public async Task<Result> Handle(DeleteJobCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs
            .Include(job => job!.Company)
            .Where(job => job.Id == command.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result.Error();

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == job.CompanyId
                    && ucc.UserId == currentUserId
                    && ucc.ClaimId == CompanyClaim.CanEditJobs.Id)
                .AnyAsync();

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();
        
        context.Jobs.Remove(job);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}