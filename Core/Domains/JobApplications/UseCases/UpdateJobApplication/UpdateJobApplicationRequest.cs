using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplication;

public record UpdateJobApplicationRequest(long JobApplicationId, string Status) : IRequest<Result>;