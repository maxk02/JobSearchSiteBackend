using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.DeleteFile;

public record DeleteFileRequest(long FileId) : IRequest<Result>;