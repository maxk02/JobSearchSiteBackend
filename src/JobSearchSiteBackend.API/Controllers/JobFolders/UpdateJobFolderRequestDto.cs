using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

public record UpdateJobFolderRequestDto(string? Name, string? Description) : IRequest<Result>;