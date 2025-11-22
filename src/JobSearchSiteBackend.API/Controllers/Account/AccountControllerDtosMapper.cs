using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using LogInResponse = JobSearchSiteBackend.API.Controllers.Account.Dtos.LogInResponse;

namespace JobSearchSiteBackend.API.Controllers.Account;

public class AccountControllerDtosMapper : Profile
{
    public AccountControllerDtosMapper()
    {
        CreateMap<LogInResponse, LogInResponseDto>();
    }
}