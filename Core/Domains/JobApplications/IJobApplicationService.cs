using Core.Domains.JobApplications.UseCases.AddApplication;
using Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;
using Shared.Result;

namespace Core.Domains.JobApplications;

public interface IJobApplicationService
{
    public Task<Result<long>> AddApplicationAsync(AddApplicationRequest request, CancellationToken cancellationToken = default);
    public Task<Result> UpdateApplicationFilesAsync(UpdateApplicationFilesRequest request, CancellationToken cancellationToken = default);
    public Task<Result> DeleteApplicationAsync(long id, CancellationToken cancellationToken = default);
}