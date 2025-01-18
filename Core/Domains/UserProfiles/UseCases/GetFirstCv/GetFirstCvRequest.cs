using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetFirstCv;

public record GetFirstCvRequest(long UserId) : IRequest<Result<GetFirstCvResponse>>;