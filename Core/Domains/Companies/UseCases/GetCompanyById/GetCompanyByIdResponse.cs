using Core.Domains.Companies.Dtos;

namespace Core.Domains.Companies.UseCases.GetCompanyById;

public record GetCompanyByIdResponse(CompanyDto? Company);