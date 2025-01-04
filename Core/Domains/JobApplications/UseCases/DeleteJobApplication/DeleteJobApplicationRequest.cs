using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public record DeleteJobApplicationRequest(long JobApplicationId) : IRequest<Result>;