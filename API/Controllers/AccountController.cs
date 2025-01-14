using Core.Domains.Accounts.UseCases.ChangePassword;
using Core.Domains.Accounts.UseCases.ConfirmEmail;
using Core.Domains.Accounts.UseCases.CreateAccount;
using Core.Domains.Accounts.UseCases.DeleteAccount;
using Core.Domains.Accounts.UseCases.ExtendSession;
using Core.Domains.Accounts.UseCases.GetUserSessions;
using Core.Domains.Accounts.UseCases.LogIn;
using Core.Domains.Accounts.UseCases.LogOut;
using Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;
using Core.Domains.Accounts.UseCases.SendPasswordResetLink;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    [HttpPost]
    [Route("/change-password")]
    public async Task<ActionResult> ChangePassword(
        ChangePasswordRequest request,
        [FromServices] ChangePasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/confirm-email")]
    public async Task<ActionResult> ConfirmEmail(
        ConfirmEmailRequest request,
        [FromServices] ConfirmEmailHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/register")]
    [AllowAnonymous]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccount(
        CreateAccountRequest request,
        [FromServices] CreateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/delete")]
    public async Task<ActionResult> DeleteAccount(
        DeleteAccountRequest request,
        [FromServices] DeleteAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/extend-session")]
    public async Task<ActionResult> ExtendSession(
        ExtendSessionRequest request,
        [FromServices] ExtendSessionHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("/sessions")]
    public async Task<ActionResult> GetUserSessions(
        GetUserSessionsRequest request,
        [FromServices] GetUserSessionsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<ActionResult<LogInResponse>> LogIn(
        LogInRequest request,
        [FromServices] LogInHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/logout")]
    public async Task<ActionResult> LogOut(
        LogOutRequest request,
        [FromServices] LogOutHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/reset-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword(
        ExtendSessionRequest request,
        [FromServices] ExtendSessionHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/send-email-confirmation-link")]
    public async Task<ActionResult> SendEmailConfirmationLink(
        SendEmailConfirmationLinkRequest request,
        [FromServices] SendEmailConfirmationLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/send-password-reset-link")]
    [AllowAnonymous]
    public async Task<ActionResult> SendPasswordResetLink(
        SendPasswordResetLinkRequest request,
        [FromServices] SendPasswordResetLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}