using Application.DTOs.SortFilterDTOs;
using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    //contracttypes, addresses
    Task<(IList<Job>, int)> GetNonPrivilegedResults(JobSortFilterDto sortFilterDto, CancellationToken cancellationToken);
    // permissions, tags, contracttypes, addresses
    Task<Job> GetManageModeJob(long jobId, long userId, CancellationToken cancellationToken);
    Task<(IList<Job>, int)> GetManageModeResults(JobSortFilterDto sortFilterDto, long userId, CancellationToken cancellationToken);
    
    void AttachContractType(long jobId, long contractTypeId);
    void DetachContractType(long jobId, long contractTypeId);

    void AttachTag(long jobId, long tagId);
    void DetachTag(long jobId, long tagId);
}