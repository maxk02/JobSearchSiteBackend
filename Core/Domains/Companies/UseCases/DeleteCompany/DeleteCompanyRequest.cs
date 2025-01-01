using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.DeleteCompany;

public record DeleteCompanyRequest(long CompanyId) : IRequest<Result>;