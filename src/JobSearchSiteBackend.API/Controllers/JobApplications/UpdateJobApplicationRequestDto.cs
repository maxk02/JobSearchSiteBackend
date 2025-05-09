using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public record UpdateJobApplicationRequestDto(JobApplicationStatus Status) : IRequest<Result>;