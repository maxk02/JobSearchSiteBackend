using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.AddCompanyBookmark;

public record AddCompanyBookmarkRequest(long UserId, long CompanyId) : IRequest<Result>;