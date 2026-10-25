using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Http;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.UploadFile;

public record UploadFileResult(long Id);