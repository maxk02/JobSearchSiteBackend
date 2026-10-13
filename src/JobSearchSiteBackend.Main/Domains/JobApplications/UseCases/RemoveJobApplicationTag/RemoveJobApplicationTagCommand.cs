using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.RemoveJobApplicationTag;

public record RemoveJobApplicationTagCommand(long JobApplicationId, string Tag) : IRequest<Result>;