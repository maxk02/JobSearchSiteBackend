using Domain.Entities.Users;
using Infrastructure.Persistence.EfCore.Context;
using Infrastructure.Persistence.EfCore.Repositories.Shared;

namespace Infrastructure.Persistence.EfCore.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(MyEfCoreDataContext dataDbContext) : base(dataDbContext)
    {
    }
    
    
}