using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record UpdateJobFolderRequest(string? Name, string? Description) : IRequest<Result>;