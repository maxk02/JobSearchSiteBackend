using Hangfire.Dashboard;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.AuthorizationFilters;

public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Allow all authenticated and unauthenticated users to see the Dashboard
        return true;
    }
}