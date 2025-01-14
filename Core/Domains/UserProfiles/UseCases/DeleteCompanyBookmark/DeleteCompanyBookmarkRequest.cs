using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.DeleteCompanyBookmark;

public record DeleteCompanyBookmarkRequest(long UserId, long CompanyId) : IRequest<Result>;