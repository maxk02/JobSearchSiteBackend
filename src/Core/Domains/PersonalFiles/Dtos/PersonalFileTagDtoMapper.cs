using AutoMapper;

namespace Core.Domains.PersonalFiles.Dtos;

public class PersonalFileTagDtoMapper : Profile
{
    public PersonalFileTagDtoMapper()
    {
        CreateMap<PersonalFile, PersonalFileTagDto>();
    }
}