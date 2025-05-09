using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.PersonalFiles;

public record UpdateFileRequestDto(string NewName) : IRequest<Result>;