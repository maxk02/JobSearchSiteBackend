using Core.Domains.Companies;
using Core.Domains.Jobs;
using Core.Domains.UserProfiles;
using Infrastructure.Persistence.EfCore.Context;
using Infrastructure.Persistence.EfCore.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories;

public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(MyEfCoreDataContext dataDbContext) : base(dataDbContext)
    {
    }
    
    public async Task<ICollection<Job>> GetBookmarkedJobsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var jobs = await DataDbContext.Jobs
                .Where(j => j.UsersWhoBookmarked!.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);

            return jobs;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task AddJobBookmarkAsync(long userId, long jobId, CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO JobBookmarks (UserId, JobId) VALUES {userId}, {jobId}",
                cancellationToken
            );
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveJobBookmarkAsync(long userId, long jobId, CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM JobBookmarks WHERE UserId = {userId} AND JobId = {jobId}",
                cancellationToken
            );
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<ICollection<Company>> GetBookmarkedCompaniesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var companies = await DataDbContext.Companies
                .Where(j => j.UsersWhoBookmarked!.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);

            return companies;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task AddCompanyBookmarkAsync(long userId, long companyId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO CompanyBookmarks (UserId, CompanyId) VALUES {userId}, {companyId}",
                cancellationToken
            );
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveCompanyBookmarkAsync(long userId, long companyId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM CompanyBookmarks WHERE UserId = {userId} AND CompanyId = {companyId}",
                cancellationToken
            );
        }
        catch (Exception e)
        {
            throw;
        }
    }
}