using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;

public record UpdateJobFolderCommand(long Id, string? Name, string? Description) : IRequest<Result>;