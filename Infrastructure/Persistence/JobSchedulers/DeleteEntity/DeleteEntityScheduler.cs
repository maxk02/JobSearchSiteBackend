using Core.Domains._Shared.Entities;
using Core.Domains._Shared.JobSchedulers;
using Quartz;

namespace Infrastructure.Persistence.JobSchedulers.DeleteEntity;

public class DeleteEntityScheduler<TEntity>(IScheduler scheduler) : IDeleteEntityScheduler<TEntity> where TEntity : BaseEntity
{
    public async Task ScheduleDeletionAsync(long entityId)
    {
        var job = JobBuilder.Create<EntityDeletionJob<TEntity>>()
            .WithIdentity($"DeleteJob_{entityId}")
            .UsingJobData("EntityId", entityId.ToString())
            .Build();

        var trigger = TriggerBuilder.Create()
            .StartAt(DateTimeOffset.Now.AddHours(1))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}