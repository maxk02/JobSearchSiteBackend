namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record AddCompanyRequest(string Name, string? Description,
    string Nip, long CountryId);