using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateApplicationStatus;

public class UpdateApplicationStatusHandler(
    ICurrentAccountService currentAccountService,
    IJobApplicationRepository jobApplicationRepository) : IRequestHandler<UpdateApplicationStatusRequest, Result>
{
    public async Task<Result> Handle(UpdateApplicationStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.ApplicationId, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();
        //todo
        jobApplication.Status = request.Status;

        await jobApplicationRepository.UpdateAsync(jobApplication, cancellationToken);

        return Result.Success();
    }
}