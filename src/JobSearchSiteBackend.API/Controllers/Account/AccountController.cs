using JobSearchSiteBackend.API.Attributes;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Account.Dtos;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.DeleteAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogOut;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResendEmailConfirmationLink;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetForgottenPassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Account;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("/change-password")]
    public async Task<ActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] ChangePasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<ChangePasswordCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/confirm-email")]
    public async Task<ActionResult> ConfirmEmail(
        [FromBody] ConfirmEmailRequest request,
        [FromServices] ConfirmEmailHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<ConfirmEmailCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccount(
        [FromBody] CreateAccountRequest request,
        [FromServices] CreateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<CreateAccountCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<CreateAccountResponse>(x)));
    }
    
    [HttpPost]
    public async Task<ActionResult> DeleteAccount(
        [FromServices] DeleteAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = new DeleteAccountCommand();
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<ActionResult<LogInResponse>> LogIn(
        [FromBody] LogInRequest request,
        [FromServices] LogInHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<LogInCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        if (!result.IsSuccess)
            return this.ToActionResult(result.Map(x => mapper.Map<LogInResponse>(x)));
        
        var logInResponseDto = mapper.Map<LogInResponse>(result.Value);
        
        var newResultWithDto = Result.Success(logInResponseDto);
        
        return this.ToActionResult(newResultWithDto);
    }
    
    [HttpPost]
    [Route("/logout")]
    public async Task<ActionResult> LogOut(
        [FromServices] LogOutHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = new LogOutCommand();
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/reset-forgotten-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetForgottenPassword(
        [FromBody] ResetForgottenPasswordRequest request,
        [FromServices] ResetForgottenPasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<ResetForgottenPasswordCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/send-email-confirmation-link")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult> SendEmailConfirmationLink(
        [FromServices] ResendEmailConfirmationLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = new ResendEmailConfirmationLinkCommand();
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/send-password-reset-link")]
    [AllowAnonymous]
    public async Task<ActionResult> SendPasswordResetLink(
        [FromBody] SendPasswordResetLinkRequest request,
        [FromServices] SendPasswordResetLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<SendPasswordResetLinkCommand>(request);
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
}