namespace Core.Services.FileStorage.JobSchedulers;

public interface IDeleteFileFromStorageScheduler
{
    public Task ScheduleAsync(Guid guidIdentifier);
}