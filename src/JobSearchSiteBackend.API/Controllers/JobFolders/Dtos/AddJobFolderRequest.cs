using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record AddJobFolderRequest(long CompanyId, long ParentId,
    string? Name, string? Description) : IRequest<Result<long>>;