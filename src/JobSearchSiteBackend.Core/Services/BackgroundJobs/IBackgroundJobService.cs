using System.Linq.Expressions;

namespace JobSearchSiteBackend.Core.Services.BackgroundJobs;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Action> methodCall, string queue, int retries = 0, TimeSpan? retryInterval = null);
    string ContinueWith(string jobId, Expression<Action> methodCall, string queue, int retries = 0);
    string Schedule(Expression<Action> methodCall, TimeSpan delay);
    void AddOrUpdateRecurring(string jobId, Expression<Action> methodCall, string cronExpression);
}