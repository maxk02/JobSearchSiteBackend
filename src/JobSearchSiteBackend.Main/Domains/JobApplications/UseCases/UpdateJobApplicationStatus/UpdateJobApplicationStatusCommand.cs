using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusCommand(long Id, JobApplicationStatus Status) : IRequest<Result>;