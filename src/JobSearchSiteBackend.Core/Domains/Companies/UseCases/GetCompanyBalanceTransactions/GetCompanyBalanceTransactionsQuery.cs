using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalanceTransactions;

public record GetCompanyBalanceTransactionsQuery(long CompanyId, int Page, int Size) : IRequest<Result<GetCompanyBalanceTransactionsResult>>;