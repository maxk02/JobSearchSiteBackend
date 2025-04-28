using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.Companies.UseCases.UpdateCompanyBalance;

public record TopUpCompanyBalanceRequest(long Id, decimal BalanceUpdate) : IRequest<Result>;