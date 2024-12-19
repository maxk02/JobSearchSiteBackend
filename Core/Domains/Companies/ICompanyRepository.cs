using Core.Domains._Shared.Repositories;
using Core.Domains.Companies.RepositoryDtos;

namespace Core.Domains.Companies;

public interface ICompanyRepository : IRepository<Company>
{
    public Task<CompanyWithPermissionIdsForUser> GetCompanyWithPermissionIdsForUser(long userId, long companyId,
        CancellationToken cancellationToken = default);
}