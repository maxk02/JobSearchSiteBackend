using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

public class PersonalFileTagDtoMapper : Profile
{
    public PersonalFileTagDtoMapper()
    {
        CreateMap<PersonalFile, PersonalFileTagDto>();
    }
}