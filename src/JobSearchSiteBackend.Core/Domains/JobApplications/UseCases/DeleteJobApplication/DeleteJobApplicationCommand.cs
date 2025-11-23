using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public record DeleteJobApplicationCommand(long Id) : IRequest<Result>;