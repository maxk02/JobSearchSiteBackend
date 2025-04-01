using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public record UpdateJobApplicationFilesRequest(long Id,
    ICollection<long> PersonalFileIds) : IRequest<Result>;