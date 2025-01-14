using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.AddCompany;

public record AddCompanyRequest(string Name, string? Description, bool IsPublic,
    long CountryId) : IRequest<Result<AddCompanyResponse>>;