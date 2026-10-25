using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.UpdateFile;

public record UpdateFileCommand(long Id, string NewName) : IRequest<Result>;