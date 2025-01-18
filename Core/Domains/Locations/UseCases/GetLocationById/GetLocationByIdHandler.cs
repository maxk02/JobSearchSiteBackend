using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Locations.UseCases.GetLocationById;

public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdRequest, Result<GetLocationByIdResponse>>
{
    public async Task<Result<GetLocationByIdResponse>> Handle(GetLocationByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}