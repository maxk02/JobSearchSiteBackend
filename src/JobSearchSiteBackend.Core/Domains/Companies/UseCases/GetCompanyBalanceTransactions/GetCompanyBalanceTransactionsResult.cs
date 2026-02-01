using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalanceTransactions;

public record GetCompanyBalanceTransactionsResult(
    ICollection<CompanyBalanceTransactionDto> CompanyBalanceTransactionDtos,
    PaginationResponse PaginationResponse);