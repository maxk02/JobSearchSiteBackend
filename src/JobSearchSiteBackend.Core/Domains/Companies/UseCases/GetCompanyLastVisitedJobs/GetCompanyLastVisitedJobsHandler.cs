using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public class GetCompanyLastVisitedJobsHandler(MainDataContext context): IRequestHandler<GetCompanyLastVisitedJobsRequest,
    Result<GetCompanyLastVisitedJobsResponse>>
{
    public async Task<Result<GetCompanyLastVisitedJobsResponse>> Handle(GetCompanyLastVisitedJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        // var stats = context.

        throw new NotImplementedException();
    }
}