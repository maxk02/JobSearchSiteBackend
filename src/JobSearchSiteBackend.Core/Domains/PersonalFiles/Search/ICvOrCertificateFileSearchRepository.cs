using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public interface ICvOrCertificateFileSearchRepository : ISearchRepository<CvOrCertificateFileSearchModel>,
    IUpdatableSearchRepository<CvOrCertificateFileSearchModel>;