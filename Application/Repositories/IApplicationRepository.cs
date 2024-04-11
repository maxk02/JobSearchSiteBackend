using Application.DTOs.SortFilterDTOs;
using Domain.Entities;
using Application.Repositories.Common;
using Application.DTOs.RepositoryDTOs;

namespace Application.Repositories;

public interface IApplicationRepository : IBaseRepository<MyApplication>
{
    Task<IList<MyApplication>> GetPaginatedResults(long jobId, ApplicationSortFilterDto sortFilterDto);
    
    Task<IList<MyFileInfoDto>> GetAllFileInfos(long applicationId);
    void AttachFile(long applicationId, long fileId);
    void DetachFile(long applicationId, long fileId);
}