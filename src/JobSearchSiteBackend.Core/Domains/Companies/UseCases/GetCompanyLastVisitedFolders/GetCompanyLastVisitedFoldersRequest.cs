using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;

public record GetCompanyLastVisitedFoldersRequest(long CompanyId): IRequest<Result<GetCompanyLastVisitedFoldersResponse>>;