using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;

public record UpdateCompanyRequest(long Id, string? Name, string? Description, bool? IsPublic) : IRequest<Result>;