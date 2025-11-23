using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UpdateFile;

public record UpdateFileCommand(long Id, string NewName) : IRequest<Result>;