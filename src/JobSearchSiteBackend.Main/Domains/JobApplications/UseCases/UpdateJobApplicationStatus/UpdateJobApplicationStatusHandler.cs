using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Domains.JobApplications.EmailMessageTemplates;
using JobSearchSiteBackend.Main.Services.EmailSender;
using JobSearchSiteBackend.Main.MyAppSettings.Email;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ISendApplicationStatusUpdatedEmailRunner sendApplicationStatusUpdatedEmailRunner,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings) : IRequestHandler<UpdateJobApplicationStatusCommand, Result>
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
                CompanyId = ja.Job!.CompanyId,
                EmailAdress = ja.User!.Account!.Email!,
                JobTitle = ja.Job!.Title,
                CompanyName = ja.Job!.Company!.Name
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationWithRelatedData is null)
            return Result.NotFound();

        var jobApplication = jobApplicationWithRelatedData.JobApplication;
        var companyId = jobApplicationWithRelatedData.CompanyId;
        var emailAddress = jobApplicationWithRelatedData.EmailAdress;
        var jobTitle = jobApplicationWithRelatedData.JobTitle;
        var companyName = jobApplicationWithRelatedData.CompanyName;

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == companyId
                    && ucc.UserId == currentUserId
                    && ucc.ClaimId == CompanyClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
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