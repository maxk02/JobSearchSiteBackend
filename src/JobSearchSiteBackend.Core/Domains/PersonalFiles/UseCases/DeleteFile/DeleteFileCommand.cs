using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.DeleteFile;

public record DeleteFileCommand(long Id) : IRequest<Result>;