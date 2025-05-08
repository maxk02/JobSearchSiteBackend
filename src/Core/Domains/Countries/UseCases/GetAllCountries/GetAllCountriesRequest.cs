using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Countries.UseCases.GetAllCountries;

public record GetAllCountriesRequest : IRequest<Result<ICollection<GetAllCountriesResponse>>>;