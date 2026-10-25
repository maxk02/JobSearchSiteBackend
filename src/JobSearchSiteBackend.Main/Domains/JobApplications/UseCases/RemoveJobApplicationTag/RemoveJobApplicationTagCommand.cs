using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.RemoveJobApplicationTag;

public record RemoveJobApplicationTagCommand(long JobApplicationId, string Tag) : IRequest<Result>;