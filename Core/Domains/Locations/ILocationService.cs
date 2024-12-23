using Core.Domains.Locations.UseCases.GetLocationById;
using Shared.Result;

namespace Core.Domains.Locations;

public interface ILocationService
{
    public Task<Result<GetLocationByIdResponse>> GetLocationByIdAsync(long id, CancellationToken cancellationToken = default);
}