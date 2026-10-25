using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.DeleteJobApplication;

public record DeleteJobApplicationCommand(long Id) : IRequest<Result>;