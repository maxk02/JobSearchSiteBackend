using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.PersonalFiles;

public record UpdateFileRequestDto(string NewName) : IRequest<Result>;