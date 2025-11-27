namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record UpdateCompanyRequest(string? Name, string? Description, bool? IsPublic);