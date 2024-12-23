using Core.Domains._Shared.Repositories;
using Shared.Result;

namespace Core.Domains.JobApplications;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    public Task<Result> AddAttachedFileIdsAsync(long jobApplicationId, ICollection<long> fileIds,
        CancellationToken cancellationToken = default);
    public Task<Result> RemoveAllAttachedFileIdsAsync(long jobApplicationId,
        CancellationToken cancellationToken = default);
}