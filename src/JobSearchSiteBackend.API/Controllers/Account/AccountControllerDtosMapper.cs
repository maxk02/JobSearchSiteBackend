using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

namespace JobSearchSiteBackend.API.Controllers.Account;

public class AccountControllerDtosMapper : Profile
{
    public AccountControllerDtosMapper()
    {
        CreateMap<LogInResponse, LogInResponseDto>();
    }
}