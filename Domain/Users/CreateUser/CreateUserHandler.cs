using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.Users.CreateUser;

public class CreateUserHandler(IUserRepository userRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null || currentAccountId != request.AccountId)
            return Result.Forbidden();
        
        var createUserResult = User.Create(request.AccountId, request.FirstName, request.MiddleName, request.LastName,
            request.DateOfBirth, request.Email, request.Phone, request.Bio);

        if (createUserResult.Value is null) return Result<CreateUserResponse>.WithMetadataFromResult(createUserResult);
            
        var newUser = await userRepository.AddAsync(createUserResult.Value, cancellationToken);
        
        return new CreateUserResponse(newUser.Id);
    }
}