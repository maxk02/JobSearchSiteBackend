using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Infrastructure.Persistence.SqlServer;

public class SqlServerInjectableSqlQueries : IInjectableSqlQueries
{
    public FormattableString GetCompanyBalanceTransactionsWithRowLocks(long companyId) =>
        $"""
            SELECT *
            FROM CompanyBalanceTransactions WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
            WHERE CompanyId = {companyId}
        """;
}