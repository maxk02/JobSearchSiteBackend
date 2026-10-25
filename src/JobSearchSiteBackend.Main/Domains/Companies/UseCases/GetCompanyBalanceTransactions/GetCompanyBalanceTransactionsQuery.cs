using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyBalanceTransactions;

public record GetCompanyBalanceTransactionsQuery(long CompanyId, int Page, int Size) : IRequest<Result<GetCompanyBalanceTransactionsResult>>;