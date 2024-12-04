using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.Users.CreateUser;

public class CreateUserHandler(IUserRepository userRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        
        
        var createUserResult = User.Create(request.FirstName, request.MiddleName, request.LastName,
            request.DateOfBirth, request.Email, request.Phone, request.Bio);

        if (createUserResult.Value is null) return Result<CreateUserResponse>.WithMetadataFromResult(createUserResult);
            
        var newUser = await userRepository.AddAsync(createUserResult.Value, cancellationToken);
        
        return new CreateUserResponse(newUser.Id);
    }
}