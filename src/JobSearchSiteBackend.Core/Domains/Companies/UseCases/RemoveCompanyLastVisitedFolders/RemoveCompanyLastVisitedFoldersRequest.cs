using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedFolders;

public record RemoveCompanyLastVisitedFoldersRequest(long CompanyId, long? SingleFolderId = null): IRequest<Result>;