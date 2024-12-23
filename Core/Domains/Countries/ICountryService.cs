using Core.Domains.Countries.UseCases.GetAllCountries;
using Shared.Result;

namespace Core.Domains.Countries;

public interface ICountryService
{
    public Task<Result<IEnumerable<GetAllCountriesResponse>>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
}