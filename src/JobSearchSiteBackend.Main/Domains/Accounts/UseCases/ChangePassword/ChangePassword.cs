using JobSearchSiteBackend.Main.Domains.Accounts.Caching;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ChangePassword;

public static class ChangePassword
{
    public record Request(string OldPassword, string NewPassword);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("change-password", Handle);

    public static async Task<Results<Ok, NotFound, ProblemHttpResult>> Handle(
        [FromBody] Request request,
        ICurrentAccountService currentAccountService,
        IUserSessionCacheRepository sessionCache,
        UserManager<MyIdentityUser> userManager,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        var currentTokenId = currentAccountService.GetTokenIdentifierOrThrow();

        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return TypedResults.NotFound();

        var identityResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!identityResult.Succeeded)
            return TypedResults.Problem("Failed to change password.", statusCode: StatusCodes.Status400BadRequest);

        await sessionCache.DeleteAllSessionsExceptCurrentAsync(currentUserId.ToString(), currentTokenId);

        return TypedResults.Ok();
    }
}