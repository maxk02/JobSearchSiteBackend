namespace Core.Domains.Companies.UseCases.GetCompanyById;

public record GetCompanyByIdResponse(string Name, string? Description, long CountryId);