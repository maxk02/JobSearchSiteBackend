using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Http;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileCommand(Stream FileStream, string Name, string Extension, long Size) : IRequest<Result<UploadFileResult>>;