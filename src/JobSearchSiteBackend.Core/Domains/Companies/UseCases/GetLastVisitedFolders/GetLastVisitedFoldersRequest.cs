using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetLastVisitedFolders;

public record GetLastVisitedFoldersRequest(): IRequest<GetLastVisitedFoldersResponse>;