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
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetAccountData;

namespace JobSearchSiteBackend.API.Controllers.Account;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("change-password")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] ChangePasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(request.OldPassword, request.NewPassword);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("confirm-email")]
    [AllowAnonymous]
    public async Task<ActionResult> ConfirmEmail(
        [FromBody] ConfirmEmailRequest request,
        [FromServices] ConfirmEmailHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(request.Token);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccount(
        [FromBody] CreateAccountRequest request,
        [FromServices] CreateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<CreateAccountResponse>(x)));
    }
    
    [HttpPost]
    public async Task<ActionResult> DeleteAccount(
        [FromServices] DeleteAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccountCommand();
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }

    [HttpGet]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult<GetAccountDataResponse>> GetAccountData(
        [FromServices] GetAccountDataHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountDataQuery();
        
        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetAccountDataResponse>(x)));
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LogInResponse>> LogIn(
        [FromBody] LogInRequest request,
        [FromServices] LogInHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LogInCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<LogInResponse>(x)));
    }
    
    [HttpPost]
    [Route("logout")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult> LogOut(
        [FromServices] LogOutHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LogOutCommand();
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("reset-forgotten-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetForgottenPassword(
        [FromBody] ResetForgottenPasswordRequest request,
        [FromServices] ResetForgottenPasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ResetForgottenPasswordCommand(request.Token, request.NewPassword);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("send-email-confirmation-link")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult> SendEmailConfirmationLink(
        [FromServices] ResendEmailConfirmationLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ResendEmailConfirmationLinkCommand();
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("send-password-reset-link")]
    [AllowAnonymous]
    public async Task<ActionResult> SendPasswordResetLink(
        [FromBody] SendPasswordResetLinkRequest request,
        [FromServices] SendPasswordResetLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new SendPasswordResetLinkCommand(request.Email);
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
}