using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;

public record GetJobFolderRequest(long Id) : IRequest<Result<GetJobFolderResponse>>;