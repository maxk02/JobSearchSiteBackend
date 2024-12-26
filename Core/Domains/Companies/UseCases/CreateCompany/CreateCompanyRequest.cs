using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.CreateCompany;

public record CreateCompanyRequest(string Name, string? Description, bool IsPublic,
    long CountryId) : IRequest<Result<CreateCompanyResponse>>;