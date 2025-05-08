using AutoMapper;
using Core.Domains.Accounts.UseCases.LogIn;

namespace API.Controllers.Account;

public class AccountControllerDtosMapper : Profile
{
    public AccountControllerDtosMapper()
    {
        CreateMap<LogInResponse, LogInResponseDto>();
    }
}