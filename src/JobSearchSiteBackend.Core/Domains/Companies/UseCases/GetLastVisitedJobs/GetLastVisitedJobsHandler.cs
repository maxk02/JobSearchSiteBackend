using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetLastVisitedJobs;

public class GetLastVisitedJobsHandler(MainDataContext context): IRequestHandler<GetLastVisitedJobsRequest, GetLastVisitedJobsResponse>
{
    public async Task<GetLastVisitedJobsResponse> Handle(GetLastVisitedJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        // var stats = context.

        return new GetLastVisitedJobsResponse();
    }
}