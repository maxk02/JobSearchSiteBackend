namespace Core.Domains._Shared.Search;

public interface ISearchRepositoryJobScheduler<T> where T : ISearchModel
{
    public Task ScheduleAddAsync(T searchModel);
    public Task ScheduleUpdateAsync(T searchModel);
    public Task ScheduleDeleteAsync(long id);
}