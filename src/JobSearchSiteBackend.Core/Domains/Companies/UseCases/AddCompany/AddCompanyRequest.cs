using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;

public record AddCompanyRequest(string Name, string? Description,
    string Nip, long CountryId): IRequest<Result<AddCompanyResponse>>;