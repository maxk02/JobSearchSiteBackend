using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.AddJobFolder;

public record AddJobFolderCommand(long CompanyId, long ParentId,
    string? Name, string? Description) : IRequest<Result<long>>;