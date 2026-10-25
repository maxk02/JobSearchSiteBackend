using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.DeleteFile;

public record DeleteFileCommand(long Id) : IRequest<Result>;