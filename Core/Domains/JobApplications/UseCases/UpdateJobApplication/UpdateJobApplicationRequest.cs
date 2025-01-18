using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Enums;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplication;

public record UpdateJobApplicationRequest(long JobApplicationId, JobApplicationStatusEnum Status) : IRequest<Result>;