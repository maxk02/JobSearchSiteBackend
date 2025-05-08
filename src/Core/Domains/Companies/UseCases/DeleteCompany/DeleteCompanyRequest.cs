using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Companies.UseCases.DeleteCompany;

public record DeleteCompanyRequest(long Id) : IRequest<Result>;