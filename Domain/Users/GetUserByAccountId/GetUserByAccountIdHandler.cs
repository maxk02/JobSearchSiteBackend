using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.Users.GetUserByAccountId;

public class GetUserByAccountIdHandler(IUserRepository userRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<GetUserByAccountIdResponse>> Handle(GetUserByAccountIdRequest request,
        CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetId() is null)
            return Result<GetUserByAccountIdResponse>.Unauthorized();
        
        var user = await userRepository.GetByAccountIdAsync(request.AccountId, cancellationToken);

        if (user == null) return Result<GetUserByAccountIdResponse>.NotFound();

        return new GetUserByAccountIdResponse(user.FirstName, user.MiddleName, user.LastName,
            user.DateOfBirth, user.Email, user.Phone, user.Bio);
    }
}