using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.LogOut;

public record LogOutRequest : IRequest<Result>;