using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<PersonalFileSearchModel>,
    IUpdatableSearchRepository<PersonalFileSearchModel>;