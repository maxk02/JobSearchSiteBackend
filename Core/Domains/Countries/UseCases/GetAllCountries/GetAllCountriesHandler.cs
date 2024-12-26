using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Countries.UseCases.GetAllCountries;

public class GetAllCountriesHandler(ICountryRepository countryRepository)
    : IRequestHandler<GetAllCountriesRequest, Result<ICollection<GetAllCountriesResponse>>>
{
    public async Task<Result<ICollection<GetAllCountriesResponse>>> Handle(GetAllCountriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var countries = await countryRepository.GetAll(cancellationToken);

        return Result<ICollection<GetAllCountriesResponse>>
            .Success(countries.Select(x => new GetAllCountriesResponse(x.Id, x.Code)).ToList());
    }
}