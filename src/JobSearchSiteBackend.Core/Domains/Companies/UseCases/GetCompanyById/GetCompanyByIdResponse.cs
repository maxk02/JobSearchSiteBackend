using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyById;

public record GetCompanyByIdResponse(CompanyDto? Company);