namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesResponse(long Id, string Name, string? Description, long CountryId);