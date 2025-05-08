using AutoMapper;

namespace Core.Domains.PersonalFiles.Dtos;

public class PersonalFileInfoDtoMapper : Profile
{
    public PersonalFileInfoDtoMapper()
    {
        CreateMap<PersonalFile, PersonalFileInfoDto>();
    }
}