using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.PersonalFiles;

public record UpdateFileRequestDto(string NewName) : IRequest<Result>;