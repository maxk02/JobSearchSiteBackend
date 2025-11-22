using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

public record UploadCompanyAvatarCommand(Stream AvatarStream, string Extension, long Size, long CompanyId): IRequest<Result<UploadCompanyAvatarResult>>;