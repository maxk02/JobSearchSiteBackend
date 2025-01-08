using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.UpdateFile;

public record UpdateFileRequest(long FileId, string NewName) : IRequest<Result>;