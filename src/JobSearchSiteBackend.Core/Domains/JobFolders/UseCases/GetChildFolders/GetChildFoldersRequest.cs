using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

public record GetChildFoldersRequest(long Id) : IRequest<Result<GetChildFoldersResponse>>;