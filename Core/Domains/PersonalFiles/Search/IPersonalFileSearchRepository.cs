using Core.Domains._Shared.Search;
using Core.Domains.Cvs.Search;

namespace Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<PersonalFileSearchModel>,
    IConcurrentSearchRepository<PersonalFileSearchModel>;