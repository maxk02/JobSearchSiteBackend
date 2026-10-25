using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.AddJobApplicationTag;

public record AddJobApplicationTagCommand(long JobApplicationId, string Tag) : IRequest<Result>;