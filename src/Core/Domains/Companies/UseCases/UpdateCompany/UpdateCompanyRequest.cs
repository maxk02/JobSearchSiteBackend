using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Companies.UseCases.UpdateCompany;

public record UpdateCompanyRequest(long Id, string? Name, string? Description, bool? IsPublic) : IRequest<Result>;