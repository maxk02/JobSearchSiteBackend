using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.PersonalFiles.UseCases.DeleteFile;

public record DeleteFileRequest(long Id) : IRequest<Result>;