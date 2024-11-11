using Domain.Entities.Applications;
using Domain.Entities.FileInformations;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface IApplicationRepository : IBaseRepository<Application>
{
    // Task<IList<Application>> GetResults(ApplicationSortFilterDto sortFilterDto);
    
    Task<IList<FileInformation>> GetAllFileInfosForApplication(long applicationId);
    void AttachFile(long applicationId, long fileId);
    void DetachFile(long applicationId, long fileId);
}