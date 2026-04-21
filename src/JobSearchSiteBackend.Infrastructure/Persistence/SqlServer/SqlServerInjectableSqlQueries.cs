using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Infrastructure.Persistence.SqlServer;

public class SqlServerInjectableSqlQueries : IInjectableSqlQueries
{
    public FormattableString LockCompanyRow(long companyId) =>
        $$"""
            SELECT 1
            FROM Companies WITH (UPDLOCK, ROWLOCK)
            WHERE Id = {companyId}
        """;
}