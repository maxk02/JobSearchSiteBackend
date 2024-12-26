using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.UpdateCompany;

public record UpdateCompanyRequest(long CompanyId, string? Name, string? Description, bool? IsPublic) : IRequest<Result>;