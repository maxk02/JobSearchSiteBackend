﻿using JobSearchSiteBackend.API.Attributes;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.DeleteAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ExtendSession;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetUserSessions;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogOut;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetPassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;
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
        ChangePasswordRequest request,
        [FromServices] ChangePasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/confirm-email")]
    public async Task<ActionResult> ConfirmEmail(
        ConfirmEmailRequest request,
        [FromServices] ConfirmEmailHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("/create")]
    public async Task<ActionResult> CreateAccount(
        CreateAccountRequest request,
        [FromServices] CreateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/delete")]
    public async Task<ActionResult> DeleteAccount(
        DeleteAccountRequest request,
        [FromServices] DeleteAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/extend-session")]
    public async Task<ActionResult<ExtendSessionResponse>> ExtendSession(
        ExtendSessionRequest request,
        [FromServices] ExtendSessionHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("/sessions")]
    public async Task<ActionResult<GetUserSessionsResponse>> GetUserSessions(
        GetUserSessionsRequest request,
        [FromServices] GetUserSessionsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
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
        
        if (!result.IsSuccess)
            return this.ToActionResult(result);
        
        var logInResponseDto = mapper.Map<LogInResponse>(result.Value);
        
        var newResultWithDto = Result.Success(logInResponseDto);
        
        return this.ToActionResult(newResultWithDto);
    }
    
    [HttpPost]
    [Route("/logout")]
    public async Task<ActionResult> LogOut(
        LogOutRequest request,
        [FromServices] LogOutHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/reset-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword(
        ResetPasswordRequest request,
        [FromServices] ResetPasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    [Route("/send-email-confirmation-link")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult> SendEmailConfirmationLink(
        SendEmailConfirmationLinkRequest request,
        [FromServices] SendEmailConfirmationLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
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
        
        return this.ToActionResult(result);
    }
}