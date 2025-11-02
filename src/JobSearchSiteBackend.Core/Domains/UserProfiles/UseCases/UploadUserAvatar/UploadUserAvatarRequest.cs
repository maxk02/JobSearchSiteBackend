using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;

public record UploadUserAvatarRequest(Stream AvatarStream, string Extension, long Size, long UserId): IRequest<Result<UploadUserAvatarResponse>>;