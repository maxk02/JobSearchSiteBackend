using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.UploadCompanyAvatar;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.UploadCompanyAvatar;

public record UploadCompanyAvatarCommand(Stream AvatarStream, string Extension, long Size, long CompanyId): IRequest<Result<UploadCompanyAvatarResult>>;