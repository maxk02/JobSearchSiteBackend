using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public record JobSearchModel
(
    long Id,
    long CompanyId,
    long CategoryId,
    string Title,
    string Description,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> Advantages
) : ISearchModel;