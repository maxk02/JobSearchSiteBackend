using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.DeleteJobFolder;

public record DeleteJobFolderRequest(long Id) : IRequest<Result>;