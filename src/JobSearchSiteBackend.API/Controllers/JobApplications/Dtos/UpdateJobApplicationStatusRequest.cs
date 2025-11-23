using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record UpdateJobApplicationStatusRequest(long Id, JobApplicationStatus Status) : IRequest<Result>;