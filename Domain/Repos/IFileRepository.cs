using Domain.Entities.FileInformations;

namespace Domain.Repos;

public interface IFileRepository
{
    void RemoveAllFilesForUser(long userId);
    // Task<IList<MyFileInfoDto>> GetAllFileInfosForUser(long userId);
    Task<byte[]> GetContent(long fileId);
    // Task<MyFileInfoDto> GetInfo(long fileId);
    Task<long> GetOwner(long fileId);
    void CreateForUser(FileInformation fileInfo, long userId);
    void Remove(long fileId);
}