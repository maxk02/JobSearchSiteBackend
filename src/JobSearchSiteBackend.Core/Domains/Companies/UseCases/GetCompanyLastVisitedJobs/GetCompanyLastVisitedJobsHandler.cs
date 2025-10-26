using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public class GetCompanyLastVisitedJobsHandler(MainDataContext context): IRequestHandler<GetCompanyLastVisitedJobsRequest, GetCompanyLastVisitedJobsResponse>
{
    public async Task<GetCompanyLastVisitedJobsResponse> Handle(GetCompanyLastVisitedJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        // var stats = context.

        return new GetCompanyLastVisitedJobsResponse();
    }
}