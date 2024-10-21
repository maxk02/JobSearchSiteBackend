using Domain.Entities;
using Domain.Entities.Applications;
using Domain.Entities.Fileinfos;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface IApplicationRepository : IBaseRepository<Application>
{
    // Task<IList<Application>> GetResults(ApplicationSortFilterDto sortFilterDto);
    
    Task<IList<Fileinfo>> GetAllFileInfosForApplication(long applicationId);
    void AttachFile(long applicationId, long fileId);
    void DetachFile(long applicationId, long fileId);
}