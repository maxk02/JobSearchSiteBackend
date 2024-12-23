using Core.Domains.PersonalFiles.RepositoryDtos;

namespace Core.Domains.PersonalFiles;

public interface IPersonalFileRepository
{
    public Task<ICollection<FileIdWithOwnerId>> GetFileIdsWithOwnerIdsAsync(ICollection<long> personalFileIds,
        CancellationToken cancellationToken = default);
}