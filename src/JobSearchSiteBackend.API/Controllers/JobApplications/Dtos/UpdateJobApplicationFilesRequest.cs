using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record UpdateJobApplicationFilesRequest(long Id,
    ICollection<long> PersonalFileIds) : IRequest<Result>;