using Core.Domains._Shared.Search;

namespace Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<PersonalFileSearchModel>,
    IConcurrentSearchRepository<PersonalFileSearchModel>;