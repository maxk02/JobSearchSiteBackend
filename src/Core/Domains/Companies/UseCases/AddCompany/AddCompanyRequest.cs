using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Companies.UseCases.AddCompany;

public record AddCompanyRequest(string Name, string? Description, bool IsPublic,
    long CountryId, string? LogoLink) : IRequest<Result<AddCompanyResponse>>;