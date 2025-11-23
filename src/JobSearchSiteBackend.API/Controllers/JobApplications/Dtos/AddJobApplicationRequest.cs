using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record AddJobApplicationRequest(long UserId, long JobId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddJobApplicationResponse>>;