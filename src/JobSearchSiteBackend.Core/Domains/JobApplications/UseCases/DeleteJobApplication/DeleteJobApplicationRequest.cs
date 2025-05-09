using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public record DeleteJobApplicationRequest(long Id) : IRequest<Result>;