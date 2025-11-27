using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Account.Dtos;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetForgottenPassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;
using LogInResponse = JobSearchSiteBackend.API.Controllers.Account.Dtos.LogInResponse;

namespace JobSearchSiteBackend.API.Controllers.Account;

public class AccountControllerDtosMapper : Profile
{
    public AccountControllerDtosMapper()
    {
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
        CreateMap<CreateAccountRequest, CreateAccountCommand>();
        CreateMap<CreateAccountResult, CreateAccountResponse>();
        CreateMap<LogInRequest, LogInCommand>();
        CreateMap<LogInResult, LogInResponse>();
        CreateMap<ResetForgottenPasswordRequest, ResetForgottenPasswordCommand>();
        CreateMap<SendPasswordResetLinkRequest, SendPasswordResetLinkCommand>();
    }
}