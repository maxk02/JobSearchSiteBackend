using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;

public record GetCompanySharedFoldersRootRequest(long CompanyId) : IRequest<Result<GetCompanySharedFoldersRootResponse>>;