using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public class GetBookmarkedCompaniesHandler(ICurrentAccountService currentAccountService,
    IUserProfileRepository userProfileRepository) 
    : IRequestHandler<GetBookmarkedCompaniesRequest, Result<ICollection<GetBookmarkedCompaniesResponse>>>
{
    public async Task<Result<ICollection<GetBookmarkedCompaniesResponse>>> Handle(GetBookmarkedCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null) return Result<ICollection<GetBookmarkedCompaniesResponse>>.Unauthorized();
        
        if (currentAccountId.Value != request.UserId) return Result<ICollection<GetBookmarkedCompaniesResponse>>.Forbidden();
        
        
        var bookmarkedCompanies = await userProfileRepository
            .GetBookmarkedCompaniesAsync(request.UserId, cancellationToken);

        var bookmarkedCompaniesResponse =
            bookmarkedCompanies
                .Select(x => new GetBookmarkedCompaniesResponse(x.Id, x.Name, x.Description, x.CountryId)).ToList();

        return Result<ICollection<GetBookmarkedCompaniesResponse>>.Success(bookmarkedCompaniesResponse);
    }
}