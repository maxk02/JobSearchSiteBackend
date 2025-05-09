using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Countries.UseCases.GetAllCountries;

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