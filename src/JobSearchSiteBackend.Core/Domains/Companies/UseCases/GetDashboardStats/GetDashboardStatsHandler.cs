using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetDashboardStats;

public class GetDashboardStatsHandler: IRequestHandler<GetDashboardStatsRequest, GetDashboardStatsResponse>
{
    public async Task<GetDashboardStatsResponse> Handle(GetDashboardStatsRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}