using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedFolders;

public record SearchCompanySharedFoldersRequest(long CompanyId, string Query) : IRequest<Result<SearchCompanySharedFoldersResponse>>;