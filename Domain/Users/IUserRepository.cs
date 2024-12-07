using Domain._Shared.Repositories;

namespace Domain.Users;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    public Task RemoveByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
}