using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;

public class UpdateApplicationFilesHandler(
    ICurrentAccountService currentAccountService,
    IJobApplicationRepository jobApplicationRepository,
    IRepository<PersonalFile> personalFileRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateApplicationFilesRequest, Result>
{
    public async Task<Result> Handle(UpdateApplicationFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.ApplicationId, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        var personalFiles = await personalFileRepository.GetByIdsAsync(request.PersonalFileIds, cancellationToken);
        if (!request.PersonalFileIds.All(personalFiles.Select(x => x.Id).Contains) ||
            personalFiles.Any(x => x.UserId != currentUserId))
        {
            return Result<long>.Error();
        }

        await unitOfWork.BeginAsync(cancellationToken);
        await jobApplicationRepository.RemoveAllAttachedFileIdsAsync(jobApplication.Id, cancellationToken);
        await jobApplicationRepository.AddAttachedFileIdsAsync(jobApplication.Id, request.PersonalFileIds,
            cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}