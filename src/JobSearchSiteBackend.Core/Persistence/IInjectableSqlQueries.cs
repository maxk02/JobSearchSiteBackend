namespace JobSearchSiteBackend.Core.Persistence;

public interface IInjectableSqlQueries
{
    public FormattableString GetCompanyBalanceTransactionsWithRowLocks(long companyId);
}