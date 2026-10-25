using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Domains.Accounts.Caching;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.DeleteAccount;

public static class DeleteAccount
{
    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapDelete("", Handle);

    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        ICurrentAccountService currentAccountService,
        UserManager<MyIdentityUser> userManager,
        MainDataContext context,
        IUserSessionCacheRepository sessionCache,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return TypedResults.Problem("Account not found.", statusCode: StatusCodes.Status400BadRequest);

        await sessionCache.DeleteAllSessionsAsync(currentUserId.ToString());

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
            return TypedResults.Problem("Could not delete account.", statusCode: StatusCodes.Status400BadRequest);

        var userAvatars = await context.UserAvatars
            .Where(ua => ua.UserId == currentUserId)
            .ToListAsync(cancellationToken);

        foreach (var userAvatar in userAvatars)
            userAvatar.IsDeleted = true;

        context.UserAvatars.UpdateRange(userAvatars);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return TypedResults.Ok();
    }
}