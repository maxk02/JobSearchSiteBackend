using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ResetForgottenPassword;

public static class ResetForgottenPassword
{
    public record Request(string Token, string NewPassword);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("reset-forgotten-password", Handle)
            .AllowAnonymous();

    public static async Task<Results<Ok, NotFound, ProblemHttpResult>> Handle(
        [FromBody] Request request,
        UserManager<MyIdentityUser> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(
                u => userManager
                    .VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, "ResetPassword", request.Token).Result,
                cancellationToken);

        if (user is null)
            return TypedResults.NotFound();

        var identityResult = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        return identityResult.Succeeded
            ? TypedResults.Ok()
            : TypedResults.Problem("Failed to reset password.", statusCode: StatusCodes.Status400BadRequest);
    }
}