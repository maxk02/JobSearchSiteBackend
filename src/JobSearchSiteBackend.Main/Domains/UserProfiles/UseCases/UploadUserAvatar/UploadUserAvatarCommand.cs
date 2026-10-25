using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.UploadCompanyAvatar;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.UploadUserAvatar;

public record UploadUserAvatarCommand(Stream AvatarStream, string Extension, long Size, long UserId): IRequest<Result<UploadUserAvatarResult>>;