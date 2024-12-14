using Core._Shared.Services.Auth;
using Shared.Result;

namespace Core.UserProfiles.UseCases.GetUserProfileById;

public class GetUserProfileByIdHandler(IUserProfileRepository userProfileRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<GetUserProfileByIdResponse>> Handle(GetUserProfileByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();
        
        var user = await userProfileRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (user is null) return Result<GetUserProfileByIdResponse>.NotFound();
        if (request.AccountId != currentAccountId && !user.IsPublic) return Result<GetUserProfileByIdResponse>.Forbidden();

        return new GetUserProfileByIdResponse(user.FirstName, user.MiddleName, user.LastName,
            user.DateOfBirth, user.Email, user.Phone, user.Bio);
    }
}