using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public record CompanySearchModel(long Id, byte[] Version, long CountryId, string Name, string? Description) : ISearchModel;