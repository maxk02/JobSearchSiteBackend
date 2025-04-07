namespace Core.Services.Search;

public interface IConcurrentSearchRepository<T> where T : ISearchModelWithId, ISearchModelWithDeletionDateTime
{
    public Task AddOrUpdateIfNewestAsync(T searchModel, byte[] rowVersion,
        CancellationToken cancellationToken = default);
    public Task SoftDeleteAsync(T searchModel, byte[] rowVersion, CancellationToken cancellationToken = default);
}