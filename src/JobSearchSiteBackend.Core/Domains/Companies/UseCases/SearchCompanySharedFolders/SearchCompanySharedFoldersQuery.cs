using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedFolders;

public record SearchCompanySharedFoldersQuery(long CompanyId, string Query) : IRequest<Result<SearchCompanySharedFoldersResult>>;