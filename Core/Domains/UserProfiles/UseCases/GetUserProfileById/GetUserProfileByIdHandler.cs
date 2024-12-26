using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public class GetUserProfileByIdHandler(IUserProfileRepository userProfileRepository,
    ICurrentAccountService currentAccountService) : IRequestHandler<GetUserProfileByIdRequest, Result<GetUserProfileByIdResponse>>
{
    public async Task<Result<GetUserProfileByIdResponse>> Handle(GetUserProfileByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();
        
        var user = await userProfileRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null) return Result<GetUserProfileByIdResponse>.NotFound();
        if (request.Id != currentAccountId && !user.IsPublic) return Result<GetUserProfileByIdResponse>.Forbidden();

        return new GetUserProfileByIdResponse(user.FirstName, user.MiddleName, user.LastName,
            user.DateOfBirth, user.Email, user.Phone);
    }
}