using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AcceptCompanyEmployeeInvitation;

public class AcceptCompanyEmployeeInvitationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AcceptCompanyEmployeeInvitationCommand, Result>
{
    public async Task<Result> Handle(AcceptCompanyEmployeeInvitationCommand command,
        CancellationToken cancellationToken = default)
    {
        var company = await context.Companies
            .FindAsync([command.CompanyId], cancellationToken);
        
        if (company is null)
            return Result.Error();

        var userProfile = await context.UserProfiles
            .Include(u => u.CompaniesWhereEmployed)
            .Include(u => u.CompanyEmployeeInvitationsReceived)
            .Where(u => u.Id == command.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userProfile is null)
        {
            return Result.Error();
        }

        if (userProfile.CompaniesWhereEmployed!.Select(c => c.Id).Contains(company.Id))
        {
            return Result.Error("User has already been employed.");
        }
        
        var companyInvitation = userProfile.CompanyEmployeeInvitationsReceived!
            .SingleOrDefault(ci => ci.CompanyId == company.Id);
        
        if (companyInvitation is null)
            return Result.Error("No company invitations received.");
        
        if (companyInvitation.GuidIdentifier.ToString() != command.Token)
            return Result.Forbidden("Invalid token.");
        
        companyInvitation.IsAccepted = true;
        context.CompanyEmployeeInvitations.Update(companyInvitation);
        
        var companyInvitationsToDelete = userProfile
            .CompanyEmployeeInvitationsReceived!
            .Where(ci => ci.CompanyId == company.Id
                && companyInvitation.GuidIdentifier.ToString() != command.Token)
            .ToList();
        
        context.CompanyEmployeeInvitations.RemoveRange(companyInvitationsToDelete);
        
        userProfile.CompaniesWhereEmployed!.Add(company);
        context.UserProfiles.Update(userProfile);
        
        // saving changes
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}