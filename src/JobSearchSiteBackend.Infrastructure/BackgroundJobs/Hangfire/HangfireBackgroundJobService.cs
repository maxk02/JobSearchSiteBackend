using System.Linq.Expressions;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Hangfire;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire;

public class HangfireBackgroundJobService : IBackgroundJobService
{
    public string Enqueue(Expression<Action> methodCall, string queue, int retries = 0, TimeSpan? retryInterval = null)
    {
        var jobId = BackgroundJob.Enqueue(queue, methodCall);
        
        return jobId;
    }

    public string ContinueWith(string parentJobId, Expression<Action> methodCall, string queue, int retries = 0)
    {
        var jobId = BackgroundJob.ContinueJobWith(parentJobId, queue, methodCall);
        
        return jobId;
    }

    public string Schedule(Expression<Action> methodCall, TimeSpan delay)
    {
        var jobId = BackgroundJob.Schedule(methodCall, delay);
        
        return jobId;
    }

    public void AddOrUpdateRecurring(string jobId, Expression<Action> methodCall, string cronExpression)
    {
        RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
    }
}