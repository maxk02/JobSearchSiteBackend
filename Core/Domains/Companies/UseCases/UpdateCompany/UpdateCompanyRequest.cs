namespace Core.Domains.Companies.UseCases.UpdateCompany;

public record UpdateCompanyRequest(long CompanyId, string? Name, string? Description, bool? IsPublic);