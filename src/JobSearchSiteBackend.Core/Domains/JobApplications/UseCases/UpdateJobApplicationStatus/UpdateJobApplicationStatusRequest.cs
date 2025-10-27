using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusRequest(long Id, JobApplicationStatus Status) : IRequest<Result>;