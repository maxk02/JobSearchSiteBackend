using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignOut;

public record SignOutRequest : IRequest<Result>;