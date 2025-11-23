using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;

public record GetJobFolderQuery(long Id) : IRequest<Result<GetJobFolderResult>>;