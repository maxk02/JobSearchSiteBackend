using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.JobApplications;

public record UpdateJobApplicationFilesRequestDto(ICollection<long> PersonalFileIds) : IRequest<Result>;