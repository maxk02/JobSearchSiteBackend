using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public record UpdateJobApplicationFilesRequest(long JobApplicationId,
    ICollection<long> PersonalFileIds) : IRequest<Result>;