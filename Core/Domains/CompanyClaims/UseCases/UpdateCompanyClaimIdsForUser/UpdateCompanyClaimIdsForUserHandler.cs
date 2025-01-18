using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

public class UpdateCompanyClaimIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<UpdateCompanyClaimIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyClaimIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new CompanyClaimIdCollectionValidator();
        var validationResult = await permissionsValidator.ValidateAsync(request.CompanyClaimIds, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var companyExists = await context.Companies
            .Where(c => c.Id == request.CompanyId)
            .AnyAsync(cancellationToken);

        if (!companyExists) return Result.NotFound();

        var userExists = await context.UserProfiles
            .Where(u => u.Id == request.UserId)
            .AnyAsync(cancellationToken);

        if (!userExists) return Result.NotFound();

        if (request.UserId == currentUserId)
            return Result.Error();

        var currentUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (!currentUserClaimIds.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();

        if (request.CompanyClaimIds.Except(currentUserClaimIds).Any())
            return Result.Forbidden();

        var targetUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == request.UserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (targetUserClaimIds.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();

        if (targetUserClaimIds.Contains(CompanyClaim.IsAdmin.Id)
            && !targetUserClaimIds.Contains(CompanyClaim.IsOwner.Id))
            return Result.Forbidden();

        var claimIdsToRemove =
            currentUserClaimIds
                .Except(request.CompanyClaimIds)
                .ToList();

        var targetUserCompanyClaimsToRemove =
            await context.UserCompanyClaims
                .Where(ujfc => ujfc.CompanyId == request.CompanyId
                               && ujfc.UserId == request.UserId
                               && claimIdsToRemove.Contains(ujfc.ClaimId))
                .ToListAsync(cancellationToken);

        context.UserCompanyClaims.RemoveRange(targetUserCompanyClaimsToRemove);
        context.UserCompanyClaims.AddRange(request.CompanyClaimIds
            .Select(id => new UserCompanyClaim(request.UserId, request.CompanyId, id)));
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}