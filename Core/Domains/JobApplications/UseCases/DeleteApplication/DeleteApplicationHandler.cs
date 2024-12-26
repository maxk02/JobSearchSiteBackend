using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteApplication;

public class DeleteApplicationHandler(
    ICurrentAccountService currentAccountService,
    IJobApplicationRepository jobApplicationRepository) : IRequestHandler<DeleteApplicationRequest, Result>
{
    public async Task<Result> Handle(DeleteApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        await jobApplicationRepository.RemoveAsync(jobApplication, cancellationToken);

        return Result.Success();
    }
}