using Domain.Users;
using Infrastructure.Persistence.EfCore.Context;
using Infrastructure.Persistence.EfCore.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(MyEfCoreDataContext dataDbContext) : base(dataDbContext)
    {
    }


    public async Task<User?> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await DataDbContext.Users
                .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);

            return user;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}