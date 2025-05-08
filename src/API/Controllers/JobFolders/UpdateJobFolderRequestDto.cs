using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.JobFolders;

public record UpdateJobFolderRequestDto(string? Name, string? Description) : IRequest<Result>;