using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.Persistence.JobSchedulers.DeleteEntity;

public class DeleteEntityJob<TEntity>(MyEfCoreDataContext dbContext) : IJob where TEntity : EntityBase
{
    public async Task Execute(IJobExecutionContext jobExecutionContext)
    {
        var entityId = jobExecutionContext.MergedJobDataMap.GetLong("EntityId");
        // var retryCount = context.MergedJobDataMap.GetInt("RetryCount");

        try
        {
            // Attempt deletion
            var entity = await dbContext.Set<TEntity>().FindAsync(entityId);
            if (entity != null)
                dbContext.Set<TEntity>().Remove(entity);
        }
        catch (Exception ex)
        {
            var trigger = TriggerBuilder.Create()
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Hour))
                .Build();

            await jobExecutionContext.Scheduler.ScheduleJob(jobExecutionContext.JobDetail, trigger);
            
        }
    }
}