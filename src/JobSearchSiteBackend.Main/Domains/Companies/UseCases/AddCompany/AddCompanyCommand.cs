using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;

public record AddCompanyCommand(string Name, string? Description,
    long CountryId, string CountrySpecificFieldsJson): IRequest<Result<AddCompanyResult>>;