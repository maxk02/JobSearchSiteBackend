using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.DeleteCompany;

public record DeleteCompanyCommand(long Id) : IRequest<Result>;