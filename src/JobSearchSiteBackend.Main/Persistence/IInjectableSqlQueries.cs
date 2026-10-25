namespace JobSearchSiteBackend.Main.Persistence;

public interface IInjectableSqlQueries
{
    public FormattableString LockCompanyRow(long companyId);
}