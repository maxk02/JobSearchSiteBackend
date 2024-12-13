using API.Domains._Shared.Repositories;
using API.Domains.Companies;
using API.Domains.Jobs;

namespace API.Domains.UserProfiles;

public interface IUserProfileRepository : IRepository<UserProfile>
{
    public Task<ICollection<Job>> GetBookmarkedJobsAsync(long userId, CancellationToken cancellationToken = default);
    public Task AddJobBookmarkAsync(long userId, long jobId, CancellationToken cancellationToken = default);
    public Task RemoveJobBookmarkAsync(long userId, long jobId, CancellationToken cancellationToken = default);
    
    public Task<ICollection<Company>> GetBookmarkedCompaniesAsync(long userId, CancellationToken cancellationToken = default);
    public Task AddCompanyBookmarkAsync(long userId, long companyId, CancellationToken cancellationToken = default);
    public Task RemoveCompanyBookmarkAsync(long userId, long companyId, CancellationToken cancellationToken = default);
}