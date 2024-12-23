using Core.Domains.Countries.UseCases.GetAllCountries;
using Shared.Result;

namespace Core.Domains.Countries;

public class CountryService(ICountryRepository countryRepository) : ICountryService
{
    public async Task<Result<IEnumerable<GetAllCountriesResponse>>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        var countries = await countryRepository.GetAll(cancellationToken);

        return Result<IEnumerable<GetAllCountriesResponse>>
            .Success(countries.Select(x => new GetAllCountriesResponse(x.Id, x.Code)));
    }
}