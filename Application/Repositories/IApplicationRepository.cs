using Application.DTOs.SortFilterDTOs;
using Domain.Entities;
using Application.Repositories.Common;
using Application.DTOs.DataRetrievalDTOs;

namespace Application.Repositories;

public interface IApplicationRepository : IBaseRepository<MyApplication>
{
    Task<IList<MyApplication>> GetResults(ApplicationSortFilterDto sortFilterDto);
    
    Task<IList<MyFileInfoDto>> GetAllFileInfosForApplication(long applicationId);
    void AttachFile(long applicationId, long fileId);
    void DetachFile(long applicationId, long fileId);
}