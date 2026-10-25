using JobSearchSiteBackend.Main.Persistence;

namespace JobSearchSiteBackend.Main.Persistence.SqlServer;

public class SqlServerInjectableSqlQueries : IInjectableSqlQueries
{
    public FormattableString LockCompanyRow(long companyId) =>
        $$"""
            SELECT 1
            FROM Companies WITH (UPDLOCK, ROWLOCK)
            WHERE Id = {companyId}
        """;
}