using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public record UpdateJobApplicationRequestDto(JobApplicationStatus Status) : IRequest<Result>;