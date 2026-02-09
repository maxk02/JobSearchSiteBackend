using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployeeInvitation;

public class GetCompanyEmployeeInvitationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyEmployeeInvitationQuery, Result<GetCompanyEmployeeInvitationResult>>
{
    public async Task<Result<GetCompanyEmployeeInvitationResult>> Handle(GetCompanyEmployeeInvitationQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == query.CompanyId
                          && ucc.UserId == currentAccountId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }
        
        var invitation = await context.CompanyEmployeeInvitations
            .Where(cei => cei.CompanyId == query.CompanyId
                && cei.InvitedUser!.Account!.Email! == query.InvitedUserEmail)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (invitation is null)
            return Result.NotFound();

        var invitationDto = new CompanyEmployeeInvitationDto(invitation.Id, invitation.DateTimeCreatedUtc,
            invitation.DateTimeValidUtc, invitation.IsAccepted);
        
        var result = new GetCompanyEmployeeInvitationResult(invitationDto);
        
        return Result.Success(result);
    }
}