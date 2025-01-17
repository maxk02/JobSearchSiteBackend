using Core.Domains._Shared.Search;

namespace Core.Domains.PersonalFiles.Search;

public record PersonalFileSearchModel(
    long Id,
    string? TextContent,
    DateTime? DeletionDateTimeUtc = null
) : ISearchModelWithId, ISearchModelWithDeletionDateTime;