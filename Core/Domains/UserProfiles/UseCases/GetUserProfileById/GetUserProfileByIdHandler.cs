using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public class GetUserProfileByIdHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) : IRequestHandler<GetUserProfileByIdRequest, Result<GetUserProfileByIdResponse>>
{
    public async Task<Result<GetUserProfileByIdResponse>> Handle(GetUserProfileByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = jwtCurrentAccountService.GetId();
        
        var query = context.UserProfiles
            .Include(u => u.Phone)
            .Where(u => u.Id == request.Id);
        
        var user  = await query.FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<GetUserProfileByIdResponse>.NotFound();
        
        if (request.Id != currentAccountId)
            return Result<GetUserProfileByIdResponse>.Forbidden();

        return new GetUserProfileByIdResponse(user.FirstName, user.MiddleName, user.LastName,
            user.DateOfBirth, user.Email, user.Phone);
    }
}