using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public record UpdateJobApplicationFilesRequest(long JobApplicationId,
    ICollection<long> PersonalFileIds) : IRequest<Result>;