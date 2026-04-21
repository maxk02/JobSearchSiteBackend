namespace JobSearchSiteBackend.Core.Persistence;

public interface IInjectableSqlQueries
{
    public FormattableString LockCompanyRow(long companyId);
}