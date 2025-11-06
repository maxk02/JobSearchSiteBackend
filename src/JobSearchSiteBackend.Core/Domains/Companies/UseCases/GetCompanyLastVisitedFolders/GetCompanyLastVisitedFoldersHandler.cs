using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;

public class GetCompanyLastVisitedFoldersHandler: IRequestHandler<GetCompanyLastVisitedFoldersRequest,
    Result<GetCompanyLastVisitedFoldersResponse>>
{
    public async Task<Result<GetCompanyLastVisitedFoldersResponse>> Handle(GetCompanyLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}