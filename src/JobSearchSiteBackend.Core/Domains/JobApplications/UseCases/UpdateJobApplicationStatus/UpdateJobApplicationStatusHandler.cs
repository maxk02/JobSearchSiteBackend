using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ISendApplicationStatusUpdatedEmailRunner sendApplicationStatusUpdatedEmailRunner,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderOptions> emailSenderSettings) : IRequestHandler<UpdateJobApplicationStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplicationWithRelatedData = await context.JobApplications
            .Where(ja => ja.Id == command.Id)
            .Select(ja => new
            {
                JobApplication = ja,
                JobFolderId = ja.Job!.JobFolderId,
                EmailAdress = ja.User!.Account!.Email!,
                JobTitle = ja.Job!.Title,
                CompanyName = ja.Job!.JobFolder!.Company!.Name
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationWithRelatedData is null)
            return Result.NotFound();

        var jobApplication = jobApplicationWithRelatedData.JobApplication;
        var jobFolderId = jobApplicationWithRelatedData.JobFolderId;
        var emailAddress = jobApplicationWithRelatedData.EmailAdress;
        var jobTitle = jobApplicationWithRelatedData.JobTitle;
        var companyName = jobApplicationWithRelatedData.CompanyName;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(jobFolderId, currentUserId,
                    JobFolderClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();
        
        context.JobApplications.Attach(jobApplication);

        jobApplication.Status = command.Status;

        context.JobApplications.Update(jobApplication);

        var emailTemplate = new JobApplicationStatusChangeEmail(command.Status, jobTitle, companyName);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);
        
        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, emailAddress,
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);
        
        await context.SaveChangesAsync(cancellationToken);

        await sendApplicationStatusUpdatedEmailRunner.RunAsync(emailToSend);

        return Result.Success();
    }
}