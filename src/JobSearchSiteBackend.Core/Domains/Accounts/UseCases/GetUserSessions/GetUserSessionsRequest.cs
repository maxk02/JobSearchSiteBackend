using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetUserSessions;

public record GetUserSessionsRequest : IRequest<Result<GetUserSessionsResponse>>;