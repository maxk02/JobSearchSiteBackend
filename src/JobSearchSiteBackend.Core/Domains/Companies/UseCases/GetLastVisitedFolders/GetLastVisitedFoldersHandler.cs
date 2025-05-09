using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetDashboardStats;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetLastVisitedFolders;

public class GetLastVisitedFoldersHandler: IRequestHandler<GetLastVisitedFoldersRequest, GetLastVisitedFoldersResponse>
{
    public async Task<GetLastVisitedFoldersResponse> Handle(GetLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}