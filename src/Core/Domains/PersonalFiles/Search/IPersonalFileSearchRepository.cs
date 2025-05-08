using Core.Services.Search;

namespace Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<PersonalFileSearchModel>,
    IConcurrentSearchRepository<PersonalFileSearchModel>;