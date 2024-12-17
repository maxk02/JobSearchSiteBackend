namespace Core.Domains.Companies.UseCases.AddCompany;

public record AddCompanyRequest(string Name, string? Description, bool IsPublic, long CountryId);