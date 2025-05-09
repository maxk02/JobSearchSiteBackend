using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Http;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileRequest(IFormFile? FormFile) : IRequest<Result>;