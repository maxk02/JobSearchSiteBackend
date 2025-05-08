using Core.Services.Search;

namespace Core.Domains.Jobs.Search;

public record JobSearchModel
(
    long Id,
    long CountryId,
    long CategoryId,
    string Title,
    string? Description,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    DateTime? DeletionDateTimeUtc = null
) : ISearchModelWithId, ISearchModelWithDeletionDateTime;