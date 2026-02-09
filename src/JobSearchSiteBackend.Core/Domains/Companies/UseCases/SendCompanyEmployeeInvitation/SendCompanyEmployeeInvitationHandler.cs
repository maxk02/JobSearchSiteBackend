using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Companies.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Companies.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SendCompanyEmployeeInvitation;

public class SendCompanyEmployeeInvitationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    UserManager<MyIdentityUser> userManager,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings,
    IOptions<MyAppSettings> appSettings,
    ISendCompanyEmployeeInvitationEmailRunner sendCompanyEmployeeInvitationEmailRunner)
    : IRequestHandler<SendCompanyEmployeeInvitationCommand, Result>
{
    public async Task<Result> Handle(SendCompanyEmployeeInvitationCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var identityUser = await userManager.FindByEmailAsync(command.InvitedUserEmail);

        if (identityUser is null)
        {
            return Result.NotFound();
        }

        var companyName = await context.Companies
            .Where(c => c.Id == command.CompanyId)
            .Select(c => c.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (companyName is null)
        {
            return Result.NotFound();
        }
        
        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == command.CompanyId
                          && ucc.UserId == currentUserId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }

        var invitation = new CompanyEmployeeInvitation(command.CompanyId, identityUser.Id, currentUserId);

        context.CompanyEmployeeInvitations.Add(invitation);
        
        await context.SaveChangesAsync(cancellationToken);

        var domainName = appSettings.Value.FrontendDomainName;
        
        var link = $"https://{domainName}/company/{command.CompanyId}/accept-invitation?token={invitation.GuidIdentifier}";

        var emailTemplate = new CompanyEmployeeInvitationEmail(link, companyName);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);
        
        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, command.InvitedUserEmail,
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);

        await sendCompanyEmployeeInvitationEmailRunner.RunAsync(emailToSend);

        return Result.Success();
    }
}