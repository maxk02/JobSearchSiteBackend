using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplication;

public record UpdateJobApplicationRequest(long Id, JobApplicationStatus Status) : IRequest<Result>;