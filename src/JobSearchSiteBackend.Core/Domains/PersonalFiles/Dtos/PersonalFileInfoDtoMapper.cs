using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

public class PersonalFileInfoDtoMapper : Profile
{
    public PersonalFileInfoDtoMapper()
    {
        CreateMap<PersonalFile, PersonalFileInfoDto>();
    }
}