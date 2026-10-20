using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ConfirmEmail;

public static class ConfirmEmail
{
    public record Request(string Token);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("confirm-email", Handle)
            .AllowAnonymous();

    public static async Task<Results<Ok, NotFound, ProblemHttpResult>> Handle(
        [FromBody] Request request,
        UserManager<MyIdentityUser> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(
                u => userManager
                    .VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, "EmailConfirmation", request.Token).Result,
                cancellationToken);

        if (user is null)
            return TypedResults.NotFound();

        var identityResult = await userManager.ConfirmEmailAsync(user, request.Token);

        return identityResult.Succeeded
            ? TypedResults.Ok()
            : TypedResults.Problem("Invalid or expired email confirmation token.", statusCode: StatusCodes.Status400BadRequest);
    }
}