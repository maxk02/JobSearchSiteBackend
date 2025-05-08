using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.UseCases.GetDashboardStats;

namespace Core.Domains.Companies.UseCases.GetLastVisitedFolders;

public class GetLastVisitedFoldersHandler: IRequestHandler<GetLastVisitedFoldersRequest, GetLastVisitedFoldersResponse>
{
    public async Task<GetLastVisitedFoldersResponse> Handle(GetLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}