using API.Domains.Companies;
using API.Services.Auth;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

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