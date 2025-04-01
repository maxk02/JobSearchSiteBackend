using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Http;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileRequest(IFormFile? FormFile) : IRequest<Result>;