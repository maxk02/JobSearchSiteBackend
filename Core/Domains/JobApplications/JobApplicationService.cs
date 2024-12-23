using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains.JobApplications.UseCases.AddApplication;
using Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;
using Core.Domains.JobApplications.UseCases.UpdateApplicationStatus;
using Core.Domains.JobApplications.Values;
using Core.Domains.PersonalFiles;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications;

public class JobApplicationService(ICurrentAccountService currentAccountService,
    IUnitOfWork unitOfWork,
    IJobApplicationRepository jobApplicationRepository,
    IPersonalFileRepository personalFileRepository) : IJobApplicationService
{
    public async Task<Result<long>> AddApplicationAsync(AddApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.UserId != currentUserId)
            return Result<long>.Forbidden();

        var creationResult = JobApplication.Create(request.UserId, request.JobId, JobApplicationStatuses.Submitted);
        if (creationResult.IsFailure)
            return Result<long>.WithMetadataFrom(creationResult);
        var jobApplication = creationResult.Value;

        var fileIdsWithOwnerIds = await personalFileRepository
            .GetFileIdsWithOwnerIdsAsync(request.PersonalFileIds, cancellationToken);
        if (!request.PersonalFileIds.All(fileIdsWithOwnerIds.Select(x => x.FileId).Contains) ||
            fileIdsWithOwnerIds.Any(x => x.OwnerId != currentUserId))
        {
            return Result<long>.Error();
        }

        await unitOfWork.BeginAsync(cancellationToken);
        jobApplication = await jobApplicationRepository.AddAsync(jobApplication, cancellationToken);
        await jobApplicationRepository.AddAttachedFileIdsAsync(jobApplication.Id, request.PersonalFileIds, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return Result<long>.Success(jobApplication.Id);
    }

    public async Task<Result> UpdateApplicationFilesAsync(UpdateApplicationFilesRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.ApplicationId, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();
        
        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        var fileIdsWithOwnerIds = await personalFileRepository
            .GetFileIdsWithOwnerIdsAsync(request.PersonalFileIds, cancellationToken);
        if (!request.PersonalFileIds.All(fileIdsWithOwnerIds.Select(x => x.FileId).Contains) ||
            fileIdsWithOwnerIds.Any(x => x.OwnerId != currentUserId))
        {
            return Result.Error();
        }

        await unitOfWork.BeginAsync(cancellationToken);
        await jobApplicationRepository.RemoveAllAttachedFileIdsAsync(jobApplication.Id, cancellationToken);
        await jobApplicationRepository.AddAttachedFileIdsAsync(jobApplication.Id, request.PersonalFileIds, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
    
    public async Task<Result> UpdateApplicationStatusAsync(UpdateApplicationStatusRequest request, CancellationToken cancellationToken = default)
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

    public async Task<Result> DeleteApplicationAsync(long id, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(id, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();
        
        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        await jobApplicationRepository.RemoveAsync(jobApplication, cancellationToken);
        
        return Result.Success();
    }
}