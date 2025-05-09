using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Countries.UseCases.GetAllCountries;

public record GetAllCountriesRequest : IRequest<Result<ICollection<GetAllCountriesResponse>>>;