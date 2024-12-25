using Shared.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileRequest(Stream FileStream, string FileName, string ContentType) : IRequest<Result>;