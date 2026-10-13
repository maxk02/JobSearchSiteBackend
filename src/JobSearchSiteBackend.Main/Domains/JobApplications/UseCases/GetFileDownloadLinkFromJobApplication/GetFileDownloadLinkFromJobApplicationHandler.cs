using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.FileStorage;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobApplications.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;

public class GetFileDownloadLinkFromJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService,
    ISendApplicationStatusUpdatedEmailRunner sendApplicationStatusUpdatedEmailRunner,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings)
    : IRequestHandler<GetFileDownloadLinkFromJobApplicationQuery, Result<GetFileDownloadLinkFromJobApplicationResult>>
{
    public async Task<Result<GetFileDownloadLinkFromJobApplicationResult>> Handle(GetFileDownloadLinkFromJobApplicationQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplicationWithRelatedData = await context.JobApplications
            .Where(ja => ja.Id == query.JobApplicationId)
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
                    && ucc.ClaimId == CompanyClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();
        
        var fileSqlRecord = await context.PersonalFiles
            .Where(pf => pf.Id == query.PersonalFileId)
            // .Where(pf => pf.JobApplications!.Any(ja => ja.Id == query.JobApplicationId)) todo
            .SingleOrDefaultAsync(cancellationToken);
        
        if (fileSqlRecord is null)
            return Result.NotFound();
        
        if (!fileSqlRecord.IsUploadedSuccessfully)
            return Result.Error();

        var link = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.PersonalFiles,
            fileSqlRecord.GuidIdentifier, fileSqlRecord.Extension, cancellationToken);

        var response = new GetFileDownloadLinkFromJobApplicationResult(link);

        if (jobApplication.Status is not JobApplicationStatus.Submitted)
        {
            return Result.Success(response);
        }

        context.JobApplications.Attach(jobApplication);
        jobApplication.Status = JobApplicationStatus.Seen;
        context.JobApplications.Update(jobApplication);

        var emailTemplate = new JobApplicationStatusChangeEmail(JobApplicationStatus.Seen, jobTitle, companyName);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);
        
        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, emailAddress,
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);
        
        await context.SaveChangesAsync(cancellationToken);

        await sendApplicationStatusUpdatedEmailRunner.RunAsync(emailToSend);
        
        return Result.Success(response);
    }
}