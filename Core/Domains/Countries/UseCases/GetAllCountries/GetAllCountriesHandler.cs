using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.Countries.UseCases.GetAllCountries;

public class GetAllCountriesHandler(MainDataContext context)
    : IRequestHandler<GetAllCountriesRequest, Result<ICollection<GetAllCountriesResponse>>>
{
    public async Task<Result<ICollection<GetAllCountriesResponse>>> Handle(GetAllCountriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var countries = await context.Countries
            .Select(x => new GetAllCountriesResponse(x.Id, x.Code)).ToListAsync(cancellationToken);

        return Result<ICollection<GetAllCountriesResponse>>.Success(countries);
    }
}