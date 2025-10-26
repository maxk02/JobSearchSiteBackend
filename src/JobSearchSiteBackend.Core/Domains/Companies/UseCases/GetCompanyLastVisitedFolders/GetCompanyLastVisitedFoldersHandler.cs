using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;

public class GetCompanyLastVisitedFoldersHandler: IRequestHandler<GetCompanyLastVisitedFoldersRequest, GetCompanyLastVisitedFoldersResponse>
{
    public async Task<GetCompanyLastVisitedFoldersResponse> Handle(GetCompanyLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}