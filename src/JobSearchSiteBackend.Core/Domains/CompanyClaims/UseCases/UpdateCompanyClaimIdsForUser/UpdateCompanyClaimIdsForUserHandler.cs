using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.AspNetCore.Identity;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

public class UpdateCompanyClaimIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    UserManager<MyIdentityUser> userManager)
    : IRequestHandler<UpdateCompanyClaimIdsForUserCommand, Result>
{
    public async Task<Result> Handle(UpdateCompanyClaimIdsForUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new CompanyClaimIdCollectionValidator();
        var validationResult = await permissionsValidator.ValidateAsync(command.CompanyClaimIds, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var companyExists = await context.Companies
            .Where(c => c.Id == command.CompanyId)
            .AnyAsync(cancellationToken);

        if (!companyExists) return Result.NotFound();

        var userExists = await context.UserProfiles
            .Where(u => u.Id == command.UserId)
            .AnyAsync(cancellationToken);

        if (!userExists) return Result.NotFound();

        if (command.UserId == currentUserId)
            return Result.Error();

        var currentUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == command.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (!currentUserClaimIds.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();

        if (command.CompanyClaimIds.Except(currentUserClaimIds).Any())
            return Result.Forbidden();

        var targetUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == command.UserId && ucp.CompanyId == command.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (targetUserClaimIds.Contains(CompanyClaim.IsOwner.Id))
            return Result.Forbidden();
        
        if (targetUserClaimIds.Contains(CompanyClaim.IsAdmin.Id)
            && !currentUserClaimIds.Contains(CompanyClaim.IsOwner.Id))
            return Result.Forbidden();

        if (command.CompanyClaimIds.Contains(CompanyClaim.IsOwner.Id))
        {
            if (string.IsNullOrEmpty(command.PasswordForConfirmation))
                return Result.Unauthorized();
            
            var user = await userManager.FindByIdAsync(currentUserId.ToString());
            if (user is null)
                return Result.Error();
            
            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, command.PasswordForConfirmation);

            if (!isPasswordCorrect)
                return Result.Unauthorized();
            
            var ownerClaimToRemove = await context.UserCompanyClaims
                .Where(ucp => ucp.UserId == currentUserId 
                              && ucp.CompanyId == command.CompanyId 
                              && ucp.ClaimId == CompanyClaim.IsOwner.Id)
                .SingleAsync(cancellationToken);
            
            context.UserCompanyClaims.Remove(ownerClaimToRemove);
        }

        var claimIdsToRemove =
            currentUserClaimIds
                .Except(command.CompanyClaimIds)
                .ToList();

        var targetUserCompanyClaimsToRemove =
            await context.UserCompanyClaims
                .Where(ujfc => ujfc.CompanyId == command.CompanyId
                               && ujfc.UserId == command.UserId
                               && claimIdsToRemove.Contains(ujfc.ClaimId))
                .ToListAsync(cancellationToken);

        context.UserCompanyClaims.RemoveRange(targetUserCompanyClaimsToRemove);
        context.UserCompanyClaims.AddRange(command.CompanyClaimIds
            .Select(id => new UserCompanyClaim(command.UserId, command.CompanyId, id)));
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}