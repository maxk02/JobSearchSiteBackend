using System.Linq.Expressions;

namespace Core.Services.BackgroundJobService;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Action> methodCall, string queue, int retries = 0);
    string ContinueWith(string jobId, Expression<Action> methodCall, string queue, int retries = 0);
    string Schedule(Expression<Action> methodCall, TimeSpan delay);
    void AddOrUpdateRecurring(string jobId, Expression<Action> methodCall, string cronExpression);
}