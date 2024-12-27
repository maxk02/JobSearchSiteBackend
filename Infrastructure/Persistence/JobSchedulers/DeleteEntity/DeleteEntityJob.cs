using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Repositories;
using Quartz;

namespace Infrastructure.Persistence.JobSchedulers.DeleteEntity;

public class DeleteEntityJob<TEntity>(IRepository<TEntity> repository) : IJob where TEntity : EntityBase
{
    public async Task Execute(IJobExecutionContext context)
    {
        var entityId = context.MergedJobDataMap.GetLong("EntityId");
        // var retryCount = context.MergedJobDataMap.GetInt("RetryCount");

        try
        {
            // Attempt deletion
            await repository.RemoveByIdAsync(entityId);
        }
        catch (Exception ex)
        {
            var trigger = TriggerBuilder.Create()
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Hour))
                .Build();

            await context.Scheduler.ScheduleJob(context.JobDetail, trigger);
            
        }
    }
}