using System.Linq.Expressions;

namespace Core.Services.QueueService;

public interface IBackgroundJobQueueService
{
    Task EnqueueForIndefiniteRetriesAsync(Expression<Action> methodCall, string queue);
    Task EnqueueForIndefiniteRetriesAsync<TService>(Expression<Action<TService>> methodCall, string queue = nameof(TService));
}