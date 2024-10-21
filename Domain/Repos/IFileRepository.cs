using Domain.Entities;
using Domain.Entities.Fileinfos;

namespace Domain.Repos;

public interface IFileRepository
{
    void RemoveAllFilesForUser(long userId);
    // Task<IList<MyFileInfoDto>> GetAllFileInfosForUser(long userId);
    Task<byte[]> GetContent(long fileId);
    // Task<MyFileInfoDto> GetInfo(long fileId);
    Task<long> GetOwner(long fileId);
    void CreateForUser(Fileinfo fileinfo, long userId);
    void Remove(long fileId);
}