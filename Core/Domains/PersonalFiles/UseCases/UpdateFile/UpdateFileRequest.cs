using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.PersonalFiles.UseCases.UpdateFile;

public record UpdateFileRequest(long Id, string NewName) : IRequest<Result>;