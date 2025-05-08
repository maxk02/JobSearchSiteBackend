using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.Accounts.UseCases.GetUserSessions;

public record GetUserSessionsRequest : IRequest<Result<GetUserSessionsResponse>>;