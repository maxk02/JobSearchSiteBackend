using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public record UpdateJobApplicationFilesRequestDto(ICollection<long> PersonalFileIds) : IRequest<Result>;