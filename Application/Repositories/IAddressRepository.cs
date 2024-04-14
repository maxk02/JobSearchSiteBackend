using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    void CreateForJob(Address address, long jobId);
    Task<IList<Address>> GetAddressesForJobId(long jobId, CancellationToken cancellationToken);
}