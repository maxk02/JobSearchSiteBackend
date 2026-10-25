using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyBalanceTransactions;

public record GetCompanyBalanceTransactionsResult(
    ICollection<CompanyBalanceTransactionDto> CompanyBalanceTransactionDtos,
    PaginationResponse PaginationResponse);