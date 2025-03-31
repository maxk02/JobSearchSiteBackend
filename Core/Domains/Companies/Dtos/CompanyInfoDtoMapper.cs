using AutoMapper;

namespace Core.Domains.Companies.Dtos;

public class CompanyInfoDtoMapper : Profile
{
    public CompanyInfoDtoMapper()
    {
        CreateMap<Company, CompanyInfoDto>();
    }
}