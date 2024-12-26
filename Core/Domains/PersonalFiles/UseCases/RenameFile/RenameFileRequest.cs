using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.RenameFile;

public record RenameFileRequest(long FileId, string NewName) : IRequest<Result>;