using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileRequest(Stream FileStream, string FileName, string Extension) : IRequest<Result>;