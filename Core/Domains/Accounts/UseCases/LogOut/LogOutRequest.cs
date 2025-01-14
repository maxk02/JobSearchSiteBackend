using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.LogOut;

public record LogOutRequest : IRequest<Result>;