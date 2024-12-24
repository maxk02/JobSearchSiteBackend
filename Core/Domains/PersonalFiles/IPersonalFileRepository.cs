using Core.Domains._Shared.Repositories;
using Core.Domains.PersonalFiles.RepositoryDtos;

namespace Core.Domains.PersonalFiles;

public interface IPersonalFileRepository : IRepository<PersonalFile>
{
    public Task<ICollection<FileIdWithOwnerId>> GetFileIdsWithOwnerIdsAsync(ICollection<long> personalFileIds,
        CancellationToken cancellationToken = default);
}