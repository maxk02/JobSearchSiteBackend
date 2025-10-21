using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public interface ITextFileSearchRepository : ISearchRepository<TextFileSearchModel>,
    IUpdatableSearchRepository<TextFileSearchModel>;