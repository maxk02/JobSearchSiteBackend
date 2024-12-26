using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.RemoveCompanyBookmark;

public record RemoveCompanyBookmarkRequest(long UserId, long CompanyId) : IRequest<Result>;