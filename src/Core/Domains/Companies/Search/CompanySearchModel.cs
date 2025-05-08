using Core.Services.Search;

namespace Core.Domains.Companies.Search;

public record CompanySearchModel(
    long Id,
    long CountryId,
    string Name,
    string? Description,
    DateTime? DeletionDateTimeUtc = null
) : ISearchModelWithId, ISearchModelWithDeletionDateTime;