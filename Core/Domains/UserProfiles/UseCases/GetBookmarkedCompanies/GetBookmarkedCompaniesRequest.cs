using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesRequest(long UserId) : IRequest<Result<ICollection<GetBookmarkedCompaniesResponse>>>;