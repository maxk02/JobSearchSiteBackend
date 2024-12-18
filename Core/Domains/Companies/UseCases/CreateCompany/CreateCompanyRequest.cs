namespace Core.Domains.Companies.UseCases.CreateCompany;

public record CreateCompanyRequest(string Name, string? Description, bool IsPublic, long CountryId);