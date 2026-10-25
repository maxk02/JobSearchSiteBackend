using JobSearchSiteBackend.Main.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies;

public static class CompanyDtoMappings
{
    public static CompanyDto ToCompanyDto(this Company company, string? avatarLink)
    {
        return new CompanyDto(company.Id, company.Name, company.Description, company.CountryId, avatarLink);
    }
}