using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<IList<Address>> GetAddressesForJobId(long jobId);
}