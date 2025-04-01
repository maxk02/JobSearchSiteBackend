using AutoMapper;

namespace Core.Domains.Accounts.Dtos;

public class UserSessionDtoMapper : Profile
{
    public UserSessionDtoMapper()
    {
        CreateMap<UserSession, UserSessionDto>();
    }
}