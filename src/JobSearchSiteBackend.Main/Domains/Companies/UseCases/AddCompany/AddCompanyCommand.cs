using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.AddCompany;

public record AddCompanyCommand(string Name, string? Description,
    long CountryId, string CountrySpecificFieldsJson): IRequest<Result<AddCompanyResult>>;