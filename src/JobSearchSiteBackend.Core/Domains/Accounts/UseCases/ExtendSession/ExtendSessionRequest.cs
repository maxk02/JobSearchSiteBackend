using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ExtendSession;

public record ExtendSessionRequest : IRequest<Result<ExtendSessionResponse>>;