using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ExtendSession;

public record ExtendSessionRequest : IRequest<Result<ExtendSessionResponse>>;