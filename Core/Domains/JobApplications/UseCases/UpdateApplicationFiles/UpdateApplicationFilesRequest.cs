using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;

public record UpdateApplicationFilesRequest(long ApplicationId, ICollection<long> PersonalFileIds) : IRequest<Result>;