using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public static class CompanyDtoMappings
{
    public static CompanyDto ToCompanyDto(this Company company, string? avatarLink)
    {
        return new CompanyDto(company.Id, company.Name, company.Description, company.CountryId, avatarLink);
    }
}