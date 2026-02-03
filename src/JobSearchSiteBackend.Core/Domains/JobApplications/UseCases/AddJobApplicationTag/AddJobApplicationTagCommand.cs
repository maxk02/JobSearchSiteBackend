using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplicationTag;

public record AddJobApplicationTagCommand(long JobApplicationId, string Tag) : IRequest<Result<AddJobApplicationTagResult>>;