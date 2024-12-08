using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.Users.AddUser;

public class AddUserHandler(IUserRepository userRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<AddUserResponse>> Handle(AddUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();
        if (currentAccountId is null || currentAccountId != request.AccountId)
            return Result<AddUserResponse>.Forbidden();
        
        var createUserResult = User.Create(request.AccountId, request.FirstName, request.MiddleName, request.LastName,
            request.DateOfBirth, request.Email, request.Phone, request.Bio);
        if (createUserResult.Value is null)
            return Result<AddUserResponse>.WithMetadataFrom(createUserResult);
        
        var addedToDbUser = await userRepository.AddAsync(createUserResult.Value, cancellationToken);
        
        return new AddUserResponse(addedToDbUser.Id);
    }
}