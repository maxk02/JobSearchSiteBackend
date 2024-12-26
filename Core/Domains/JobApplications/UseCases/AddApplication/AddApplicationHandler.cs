using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Values;
using Core.Domains.PersonalFiles;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddApplication;

public class AddApplicationHandler(
    ICurrentAccountService currentAccountService,
    IRepository<PersonalFile> personalFileRepository,
    IUnitOfWork unitOfWork,
    IJobApplicationRepository jobApplicationRepository)
    : IRequestHandler<AddApplicationRequest, Result<AddApplicationResponse>>
{
    public async Task<Result<AddApplicationResponse>> Handle(AddApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.UserId != currentUserId)
            return Result<AddApplicationResponse>.Forbidden();

        var creationResult = JobApplication.Create(request.UserId, request.JobId, JobApplicationStatuses.Submitted);
        if (creationResult.IsFailure)
            return Result<AddApplicationResponse>.WithMetadataFrom(creationResult);
        var jobApplication = creationResult.Value;

        var personalFiles = await personalFileRepository.GetByIdsAsync(request.PersonalFileIds, cancellationToken);
        if (!request.PersonalFileIds.All(personalFiles.Select(x => x.Id).Contains) ||
            personalFiles.Any(x => x.UserId != currentUserId))
        {
            return Result<AddApplicationResponse>.Error();
        }

        await unitOfWork.BeginAsync(cancellationToken);
        jobApplication = await jobApplicationRepository.AddAsync(jobApplication, cancellationToken);
        await jobApplicationRepository.AddAttachedFileIdsAsync(jobApplication.Id, request.PersonalFileIds,
            cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<AddApplicationResponse>.Success(new AddApplicationResponse(jobApplication.Id));
    }
}