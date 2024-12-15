using Core.Domains.Companies;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public class GetBookmarkedCompaniesHandler(ICurrentAccountService currentAccountService,
    IUserProfileRepository userProfileRepository)
{
    public async Task<Result<ICollection<Company>>> Handle(GetBookmarkedCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null) return Result<ICollection<Company>>.Unauthorized();
        
        if (currentAccountId.Value != request.UserId) return Result<ICollection<Company>>.Forbidden();
        
        
        var bookmarkedCompanies = await userProfileRepository
            .GetBookmarkedCompaniesAsync(request.UserId, cancellationToken);

        return Result<ICollection<Company>>.Success(bookmarkedCompanies);
    }
}