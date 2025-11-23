using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

public record GetChildFoldersQuery(long Id) : IRequest<Result<GetChildFoldersResult>>;